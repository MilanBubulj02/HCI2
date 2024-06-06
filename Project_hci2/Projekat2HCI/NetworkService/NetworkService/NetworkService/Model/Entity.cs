using NetworkService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
	public class Entity : BindableBase
	{
		private int id;
		private string name;
		private EntityType type;
		private double _value;
		private bool _isSelected;
		List<Pair<DateTime, double>> _valueList;

		public int Id
		{
			get { return id; }
			set
			{
				if (id != value)
				{
					id = value;
					OnPropertyChanged("Id");
				}
			}
		}
		public string Name
		{
			get { return name; }
			set
			{
				if (name != value)
				{
					name = value;
					OnPropertyChanged("Name");
				}
			}
		}

		public EntityType Type
		{
			get { return type; }
			set
			{
				if (type != value)
				{
					type = value;
					OnPropertyChanged("Type");
				}
			}
		}

		public double Value
		{
			get
			{
				return _value;
			}
			set
			{
				if (_value != value)
				{
					_value = value;
					OnPropertyChanged(nameof(Value));
				}
			}
		}
		public bool IsSelected
		{
			get { return _isSelected; }
			set
			{
				if (_isSelected != value)
				{
					_isSelected = value;
					OnPropertyChanged(nameof(IsSelected));
				}
			}
		}

		public List<Pair<DateTime, double>> ValueList
        {
			get
			{
				return _valueList;
			}
			set
			{
				if (_valueList != value)
				{
                    _valueList = value;
					OnPropertyChanged(nameof(ValueList));
				}
			}
		}

		public Entity()
		{
            ValueList = new List<Pair<DateTime, double>>();
		}
		public Entity(int id, string name, Type type)
		{
			Id = id;
			Name = name;
			Type = new EntityType(type);
            ValueList = new List<Pair<DateTime, double>>();

		}
		public override string ToString()
		{
			return $"Entity [Id={id}, Name={name}, Type={type}, Value={_value}]";
		}
	}
}
