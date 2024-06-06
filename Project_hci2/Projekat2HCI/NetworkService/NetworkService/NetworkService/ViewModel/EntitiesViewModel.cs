using NetworkService.Helpers;
using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMLight.Messaging;
using System.Windows.Media;
using Notification.Wpf;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Runtime.InteropServices;



namespace NetworkService.ViewModel
{
	public class EntitiesViewModel : BindableBase
	{
		private string _idText;
		private string _nameText;
		private bool _isGreaterChecked;
		private bool _isLessChecked;
		private bool _isEqualChecked;
		private string _filterType;
		private bool _intervalChecked;
		private bool _smartChecked;
		private string _idTextBox;
		private ObservableCollection<Entity> _entities;
		private ObservableCollection<Entity> filter = new ObservableCollection<Entity>();
        private ObservableCollection<Entity> filterHolder = new ObservableCollection<Entity>();
        private Entity currentEntity;

        public List<string> IsTypes { get; set; }

        public string IdTextBox
        {
			get { return _idTextBox; }
			set { _idTextBox = value; OnPropertyChanged(nameof(IdTextBox)); }
		}

        public string FilterType
        {
            get { return _filterType; }
            set
            {
                if (_filterType != value)
                {
                    _filterType = value;
                    OnPropertyChanged(nameof(FilterType));
                }
            }
        }
        public bool IsGreaterChecked
		{
			get { return _isGreaterChecked; }
			set
			{
				if (_isGreaterChecked != value)
				{
					_isGreaterChecked = value;
					OnPropertyChanged(nameof(IsGreaterChecked));
				}
			}
		}
		public bool IsLesserChecked
		{
			get { return _isLessChecked; }
			set
			{
				if (_isLessChecked != value)
				{
					_isLessChecked = value;
					OnPropertyChanged(nameof(IsLesserChecked));
				}
			}
		}

        public bool IsEqualChecked
        {
            get { return _isEqualChecked; }
            set
            {
                if (_isEqualChecked != value)
                {
                    _isEqualChecked = value;
                    OnPropertyChanged(nameof(IsEqualChecked));
                }
            }
        }


        public bool IntervalChecked
        {
			get { return _intervalChecked; }
			set
			{
				if (_intervalChecked != value)
				{
                    _intervalChecked = value;
					OnPropertyChanged(nameof(IntervalChecked));
				}
			}
		}

		public bool SmartChecked
		{
			get { return _smartChecked; }
			set
			{
				if (_smartChecked != value)
				{
                    _smartChecked = value;
					OnPropertyChanged(nameof(SmartChecked));
				}
			}
		}

		public string IdText
		{
			get { return _idText; }
			set { _idText = value; OnPropertyChanged(nameof(IdText)); }
		}

		public string NameText
		{
			get { return _nameText; }
			set { _nameText = value; OnPropertyChanged(nameof(NameText)); }
		}

		public MyICommand AddEntityCommand { get; set; }
		public MyICommand DeleteEntityCommand { get; set; }
        public MyICommand FilterCommand { get; set; }
        public MyICommand ClearFiltersCommand { get; set; }
        public MyICommand ShowEntityCommand { get; set; }

		public ObservableCollection<Entity> _Entities
		{
			get { return _entities; }
			set { _entities = value; OnPropertyChanged(nameof(_Entities)); }
		}

		public Entity CurrentEntity
		{
			get { return currentEntity; }
			set
			{
				currentEntity = value;
				OnPropertyChanged(nameof(CurrentEntity));
			}
		}

		public EntitiesViewModel()
		{
			AddEntityCommand = new MyICommand(OnAdd);
			DeleteEntityCommand = new MyICommand(OnDelete);
            FilterCommand = new MyICommand(OnFilter);
			ClearFiltersCommand = new MyICommand(OnClear);
            _Entities = MainWindowViewModel.Entities;
            IntervalChecked = true;
			IdTextBox = string.Empty;

            IsTypes = new List<string>
            {
                "IntervalMeter",
                "SmartMeter",
				"Any"
            };
        }

		public void OnAdd()
		{
			if (!int.TryParse(_idText, out _))
			{
				IdText = string.Empty;
				return;
			}

			if (MainWindowViewModel.Entities.Any(e => e.Id == int.Parse(_idText)))
			{
				IdText = string.Empty;
				return;
			}

			Entity entity = new Entity
			{
				Id = int.Parse(_idText),
				Name = _nameText,
			};

			if (IntervalChecked) 
			{
				entity.Type = new EntityType(Model.Type.IntervalMeter);
			}
			else
			{
				entity.Type = new EntityType(Model.Type.SmartMeter);
			}

			MainWindowViewModel.Entities.Add(entity);
			
			if (IntervalChecked)
			{
				MainWindowViewModel.Interval_Entities.Add(entity);
			}
			else
			{
				MainWindowViewModel.Smart_Entities.Add(entity);
			}
			ResetFields();
		}

		private void ResetFields()
		{
			IdText = string.Empty;
			NameText = string.Empty;
            IntervalChecked = true;
            SmartChecked = false;
		}

		public void OnDelete()
		{
			MessageBoxResult result = MessageBox.Show(
				"Are you sure you want to delete the selected items?",
				"Confirm Deletion",
				MessageBoxButton.YesNo,
				MessageBoxImage.Warning
			);

			if (result == MessageBoxResult.No)
				return;

			var itemsToDelete = _Entities.Where(e => e.IsSelected).ToList();
			foreach (var item in itemsToDelete)
			{
				MainWindowViewModel.Entities.Remove(item);
				try { filter.Remove(item); } catch { }
			}
			if (IdTextBox.Equals(string.Empty))
			{
				_Entities = MainWindowViewModel.Entities;
			}
			else
			{
				_Entities = filter;
			}
		}

        public void OnFilter()
        {
            filter = new ObservableCollection<Entity>();
            filterHolder = new ObservableCollection<Entity>();

            Entity checker = new Entity
            {
                Id = 100001,
                Name = "holder",
            };
            checker.Type = new EntityType(Model.Type.IntervalMeter);

			if (FilterType == IsTypes[0])
			{
				if (IdTextBox.Equals(string.Empty))
				{
					foreach (Entity e in MainWindowViewModel.Entities)
					{
						if (e.Type == checker.Type)
							filter.Add(e);
					}
				}
				else
				{
					if (IsEqualChecked)
					{
						foreach (Entity e in MainWindowViewModel.Entities)
						{
							if (e.Id == Int32.Parse(IdTextBox) && (e.Type == checker.Type))
								filter.Add(e);
						}
					}
					else if (IsGreaterChecked)
					{
						foreach (Entity e in MainWindowViewModel.Entities)
						{
							if (e.Id > Int32.Parse(IdTextBox) && (e.Type == checker.Type))
								filter.Add(e);
						}
					}
					else if (IsLesserChecked)
					{
						foreach (Entity e in MainWindowViewModel.Entities)
						{
							if (e.Id < Int32.Parse(IdTextBox) && (e.Type == checker.Type))
								filter.Add(e);
						}
					}
				}
			}
			else if (FilterType == IsTypes[1])
			{
				if (IdTextBox.Equals(string.Empty))
				{
					foreach (Entity e in MainWindowViewModel.Entities)
					{
						if (e.Type != checker.Type)
							filter.Add(e);
					}
				}
				else
				{
					if (IsEqualChecked)
					{
						foreach (Entity e in MainWindowViewModel.Entities)
						{
							if (e.Id == Int32.Parse(IdTextBox) && (e.Type != checker.Type))
								filter.Add(e);
						}
					}
					else if (IsGreaterChecked)
					{
						foreach (Entity e in MainWindowViewModel.Entities)
						{
							if (e.Id > Int32.Parse(IdTextBox) && (e.Type != checker.Type))
								filter.Add(e);
						}
					}
					else if (IsLesserChecked)
					{
						foreach (Entity e in MainWindowViewModel.Entities)
						{
							if (e.Id < Int32.Parse(IdTextBox) && (e.Type != checker.Type))
								filter.Add(e);
						}
					}
				}
			}
			else
			{
				if (IdTextBox.Equals(string.Empty))
				{
					foreach (Entity e in MainWindowViewModel.Entities)
					{
							filter.Add(e);
					}
				}
				else
				{
					if (IsEqualChecked)
					{
						foreach (Entity e in MainWindowViewModel.Entities)
						{
							if (e.Id == Int32.Parse(IdTextBox))
								filter.Add(e);
						}
					}
					else if (IsGreaterChecked)
					{
						foreach (Entity e in MainWindowViewModel.Entities)
						{
							if (e.Id > Int32.Parse(IdTextBox))
								filter.Add(e);
						}
					}
					else if (IsLesserChecked)
					{
						foreach (Entity e in MainWindowViewModel.Entities)
						{
							if (e.Id < Int32.Parse(IdTextBox))
								filter.Add(e);
						}
					}
				}
			}
            _Entities = filter;
        }


        public void OnClear()
        {
            IdTextBox = string.Empty;
            filter.Clear();
            _Entities = MainWindowViewModel.Entities;
        }

        public int GetCanvasIndexForEntityId(int entityId)
		{
			for (int i = 0; i < DisplayViewModel.CanvasCollection.Count; i++)
			{
				Entity entity = (DisplayViewModel.CanvasCollection[i].Resources["data"]) as Entity;

				if ((entity != null) && (entity.Id == entityId))
				{
					return i;
				}
			}
			return -1;
		}
    }
}

