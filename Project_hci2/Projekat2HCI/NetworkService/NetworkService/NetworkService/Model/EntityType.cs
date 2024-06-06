using NetworkService.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetworkService.Model
{
	public enum Type
	{
		IntervalMeter,
		SmartMeter
	}
	public class EntityType : INotifyPropertyChanged
	{
		private Type type;

		public Uri PathToTypeImage { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
		public Type Type
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

		public EntityType(Type type)
		{
			this.type = type;
			if (type == Type.IntervalMeter)
			{
				this.PathToTypeImage = new Uri("pack://application:,,,/NetworkService;component/Assets/Interval-meter.png");
			}
			else
			{
				this.PathToTypeImage = new Uri("pack://application:,,,/NetworkService;component/Assets/smart-meter.png");
			}
		}

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public override string ToString()
		{
			return Type.ToString();
		}

	}
}
