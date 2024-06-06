using NetworkService.Helpers;
using NetworkService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Threading;
using System.Windows.Shapes;

namespace NetworkService.ViewModel
{
	public class GraphViewModel : BindableBase
	{
		public ObservableCollection<Entity> Entities { get; set; }

		private Entity selectedEntity;
		public Entity SelectedEntity
		{
			get => selectedEntity;
			set
			{
				selectedEntity = value;
				UpdatePositions(null);
			}
		}

		private readonly double graphCoefficient = 0.16;
		private readonly double graphScale = 100;
		private readonly double defaultMargin = 450;
		public MyICommand SelectCommand;
		public MyICommand SelectionChangedCommand { get; }

		private readonly Timer _timer;

        #region GetSet

        private SolidColorBrush recColor_1;
        private SolidColorBrush recColor_2;
        private SolidColorBrush recColor_3;
        private SolidColorBrush recColor_4;
        private SolidColorBrush recColor_5;
        public SolidColorBrush RecColor_1 { get => recColor_1; set => SetProperty(ref recColor_1, value); }
		public SolidColorBrush RecColor_2 { get => recColor_2; set => SetProperty(ref recColor_2, value); }
		public SolidColorBrush RecColor_3 { get => recColor_3; set => SetProperty(ref recColor_3, value); }
		public SolidColorBrush RecColor_4 { get => recColor_4; set => SetProperty(ref recColor_4, value); }
		public SolidColorBrush RecColor_5 { get => recColor_5; set => SetProperty(ref recColor_5, value); }

		private double heightValue_1;
        private double heightValue_2;
        private double heightValue_3;
        private double heightValue_4;
        private double heightValue_5;
        public double HeightValue_1 { get => heightValue_1; set => SetProperty(ref heightValue_1, value); }
        public double HeightValue_2 { get => heightValue_2; set => SetProperty(ref heightValue_2, value); }
        public double HeightValue_3 { get => heightValue_3; set => SetProperty(ref heightValue_3, value); }
        public double HeightValue_4 { get => heightValue_4; set => SetProperty(ref heightValue_4, value); }
        public double HeightValue_5 { get => heightValue_5; set => SetProperty(ref heightValue_5, value); }

		private double topPoint_1;
		private double topPoint_2;
		private double topPoint_3;
		private double topPoint_4;
		private double topPoint_5;
		public double TopPoint_1 { get => topPoint_1; set => SetProperty(ref topPoint_1, value); }
		public double TopPoint_2 { get => topPoint_2; set => SetProperty(ref topPoint_2, value); }
		public double TopPoint_3 { get => topPoint_3; set => SetProperty(ref topPoint_3, value); }
		public double TopPoint_4 { get => topPoint_4; set => SetProperty(ref topPoint_4, value); }
		public double TopPoint_5 { get => topPoint_5; set => SetProperty(ref topPoint_5, value); }

        private double textTopPoint_1;
        private double textTopPoint_2;
        private double textTopPoint_3;
        private double textTopPoint_4;
        private double textTopPoint_5;
        public double TextTopPoint_1 { get => textTopPoint_1; set => SetProperty(ref textTopPoint_1, value); }
        public double TextTopPoint_2 { get => textTopPoint_2; set => SetProperty(ref textTopPoint_2, value); }
        public double TextTopPoint_3 { get => textTopPoint_3; set => SetProperty(ref textTopPoint_3, value); }
        public double TextTopPoint_4 { get => textTopPoint_4; set => SetProperty(ref textTopPoint_4, value); }
        public double TextTopPoint_5 { get => textTopPoint_5; set => SetProperty(ref textTopPoint_5, value); }

        private string recText_1;
        private string recText_2;
        private string recText_3;
        private string recText_4;
        private string recText_5;
        public string RecText_1 { get => recText_1; set => SetProperty(ref recText_1, value); }
		public string RecText_2 { get => recText_2; set => SetProperty(ref recText_2, value); }
		public string RecText_3 { get => recText_3; set => SetProperty(ref recText_3, value); }
		public string RecText_4 { get => recText_4; set => SetProperty(ref recText_4, value); }
		public string RecText_5 { get => recText_5; set => SetProperty(ref recText_5, value); }

		private string time_1;
        private string time_2;
        private string time_3;
        private string time_4;
        private string time_5;
        public string Time_1 { get => time_1; set => SetProperty(ref time_1, value); }
		public string Time_2 { get => time_2; set => SetProperty(ref time_2, value); }
		public string Time_3 { get => time_3; set => SetProperty(ref time_3, value); }
		public string Time_4 { get => time_4; set => SetProperty(ref time_4, value); }
		public string Time_5 { get => time_5; set => SetProperty(ref time_5, value); }


        #endregion

        public GraphViewModel()
		{
			Entities = MainWindowViewModel.Entities;
			if (Entities.Count != 0)
			{
				SelectedEntity = Entities[0];
			}


            RecColor_1 = new SolidColorBrush(Colors.Teal);
            RecColor_2 = new SolidColorBrush(Colors.Teal);
            RecColor_3 = new SolidColorBrush(Colors.Teal);
            RecColor_4 = new SolidColorBrush(Colors.Teal);
            RecColor_5 = new SolidColorBrush(Colors.Teal);

            RecText_1 = "-";
            RecText_2 = "-";
            RecText_3 = "-";
            RecText_4 = "-";
            RecText_5 = "-";

			Time_1 = "00:00";
			Time_2 = "00:00";
			Time_3 = "00:00";
			Time_4 = "00:00";
			Time_5 = "00:00";

			//setting up an update timer to update the graph visuals
			_timer = new Timer(UpdatePositions, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(200));
		}

        public static double RoundDownToTwoDecimals(double input)
        {
            return Math.Floor(input * 100) / 100;
        }

        public static int CastDoubleToInt(double input)
        {
            return (int)input;
        }

        private void UpdatePositions(object state)
		{
			if (SelectedEntity != null && Application.Current != null)
			{

				Application.Current.Dispatcher.Invoke(() =>
				{
					if (SelectedEntity.ValueList.Count > 0)
					{
                        double holder1 = SelectedEntity.ValueList[0].Item2;
						if((RoundDownToTwoDecimals(holder1) * graphScale) > 350)
						{
							HeightValue_1 = 350;
							TopPoint_1 = 100;
							TextTopPoint_1 = 110;
						}
						else if((RoundDownToTwoDecimals(holder1) * graphScale) <50)
						{
                            HeightValue_1 = 50;
                            TopPoint_1 = 400;
							TextTopPoint_1 = 410;
                        }
						else
						{
							double tHeightValue_1 = RoundDownToTwoDecimals(holder1) * graphScale;
							HeightValue_1 = CastDoubleToInt(tHeightValue_1);
							TopPoint_1 = 450 - CastDoubleToInt(tHeightValue_1);
							TextTopPoint_1 = 460 - CastDoubleToInt(tHeightValue_1);
                        }

                        RecText_1 = RoundDownToTwoDecimals(holder1).ToString();


						if (RoundDownToTwoDecimals(SelectedEntity.ValueList[0].Item2) < 0.34 || RoundDownToTwoDecimals(SelectedEntity.ValueList[0].Item2) > 2.73)
						{
                            RecColor_1 = new SolidColorBrush(Colors.Red);
						}
						else
						{
                            RecColor_1 = new SolidColorBrush(Colors.Green);
						}
						DateTime dateTime = SelectedEntity.ValueList[0].Item1;
						Time_1 = dateTime.Minute.ToString() + ":" + dateTime.Second.ToString();
					}
					else
					{

						RecText_1 = "-";
						Time_1 = "00:00";
					}


					if (SelectedEntity.ValueList.Count > 1)
					{

                        double holder2 = SelectedEntity.ValueList[1].Item2;
                        if ((RoundDownToTwoDecimals(holder2) * graphScale) > 350)
                        {
                            HeightValue_2 = 350;
                            TopPoint_2 = 100;
                            TextTopPoint_2 = 110;
                        }
                        else if ((RoundDownToTwoDecimals(holder2) * graphScale) < 50)
                        {
                            HeightValue_2 = 50;
                            TopPoint_2 = 400;
                            TextTopPoint_2 = 410;

                        }
                        else
                        {
                            double tHeightValue_2 = RoundDownToTwoDecimals(holder2) * graphScale;
                            HeightValue_2 = CastDoubleToInt(tHeightValue_2);
                            TopPoint_2 = 450 - CastDoubleToInt(tHeightValue_2);
                            TextTopPoint_2 = 460 - CastDoubleToInt(tHeightValue_2);
                        }

                        RecText_2 = RoundDownToTwoDecimals(holder2).ToString();

                        if (RoundDownToTwoDecimals(SelectedEntity.ValueList[1].Item2) < 0.34 || RoundDownToTwoDecimals(SelectedEntity.ValueList[1].Item2) > 2.73)
						{
                            RecColor_2 = new SolidColorBrush(Colors.Red);
						}
						else
						{
                            RecColor_2 = new SolidColorBrush(Colors.Green);
						}
						DateTime dateTime = SelectedEntity.ValueList[1].Item1;
						Time_2 = dateTime.Minute.ToString() + ":" + dateTime.Second.ToString();
					}
					else
					{

                        RecText_2 = "-";
						Time_2 = "00:00";
					}

					if (SelectedEntity.ValueList.Count > 2)
					{

                        double holder3 = SelectedEntity.ValueList[2].Item2;
                        if ((RoundDownToTwoDecimals(holder3) * graphScale) > 350)
                        {
                            HeightValue_3 = 350;
                            TopPoint_3 = 100;
                            TextTopPoint_3 = 110;
                        }
                        else if ((RoundDownToTwoDecimals(holder3) * graphScale) < 50)
                        {
                            HeightValue_3 = 50;
                            TopPoint_3 = 400;
                            TextTopPoint_3 = 410;
                        }
                        else
                        {
                            double tHeightValue_3 = RoundDownToTwoDecimals(holder3) * graphScale;
                            HeightValue_3 = CastDoubleToInt(tHeightValue_3);
                            TopPoint_3 = 450 - CastDoubleToInt(tHeightValue_3);
                            TextTopPoint_3 = 460 - CastDoubleToInt(tHeightValue_3);
                        }

                        RecText_3 = RoundDownToTwoDecimals(holder3).ToString();

                        if (RoundDownToTwoDecimals(SelectedEntity.ValueList[2].Item2) < 0.34 || RoundDownToTwoDecimals(SelectedEntity.ValueList[2].Item2) > 2.73)
						{
                            RecColor_3 = new SolidColorBrush(Colors.Red);
						}
						else
						{
                            RecColor_3 = new SolidColorBrush(Colors.Green);
						}
						DateTime dateTime = SelectedEntity.ValueList[2].Item1;
						Time_3 = dateTime.Minute.ToString() + ":" + dateTime.Second.ToString();
					}
					else
					{
                        RecText_3 = "-";
						Time_3 = "00:00";
					}

					if (SelectedEntity.ValueList.Count > 3)
					{

                        double holder4 = SelectedEntity.ValueList[3].Item2;
                        if ((RoundDownToTwoDecimals(holder4) * graphScale) > 350)
                        {
                            HeightValue_4 = 350;
                            TopPoint_4 = 100;
                            TextTopPoint_4 = 110;
                        }
                        else if ((RoundDownToTwoDecimals(holder4) * graphScale) < 50)
                        {
                            HeightValue_4 = 50;
                            TopPoint_4 = 400;
                            TextTopPoint_4 = 410;
                        }
                        else
                        {
                            double tHeightValue_4 = RoundDownToTwoDecimals(holder4) * graphScale;
                            HeightValue_4 = CastDoubleToInt(tHeightValue_4);
                            TopPoint_4 = 450 - CastDoubleToInt(tHeightValue_4);
                            TextTopPoint_4 = 460 - CastDoubleToInt(tHeightValue_4);

                        }

                        RecText_4 = RoundDownToTwoDecimals(holder4).ToString();

                        if (RoundDownToTwoDecimals(SelectedEntity.ValueList[3].Item2) < 0.34 || RoundDownToTwoDecimals(SelectedEntity.ValueList[3].Item2) > 2.73)
						{
                            RecColor_4 = new SolidColorBrush(Colors.Red);
						}
						else
						{
                            RecColor_4 = new SolidColorBrush(Colors.Green);
						}
						DateTime dateTime = SelectedEntity.ValueList[3].Item1;
						Time_4 = dateTime.Minute.ToString() + ":" + dateTime.Second.ToString();
					}
					else
					{
                        RecText_4 = "-";
						Time_4 = "00:00";
					}

					if (SelectedEntity.ValueList.Count > 4)
					{

                        double holder5 = SelectedEntity.ValueList[4].Item2;
                        if ((RoundDownToTwoDecimals(holder5) * graphScale) > 350)
                        {
                            HeightValue_5 = 350;
                            TopPoint_5 = 100;
                            TextTopPoint_5 = 110;
                        }
                        else if ((RoundDownToTwoDecimals(holder5) * graphScale) < 50)
                        {
                            HeightValue_5 = 50;
                            TopPoint_5 = 400;
                            TextTopPoint_5 = 410;
                        }
                        else
                        {
                            double tHeightValue_5 = RoundDownToTwoDecimals(holder5) * graphScale;
                            HeightValue_5 = CastDoubleToInt(tHeightValue_5);
                            TopPoint_5 = 450 - CastDoubleToInt(tHeightValue_5);
                            TextTopPoint_5 = 460 - CastDoubleToInt(tHeightValue_5);
                        }

                        RecText_5 = RoundDownToTwoDecimals(holder5).ToString();

                        if (RoundDownToTwoDecimals(SelectedEntity.ValueList[4].Item2) < 0.34 || RoundDownToTwoDecimals(SelectedEntity.ValueList[4].Item2) > 2.73)
						{
                            RecColor_5 = new SolidColorBrush(Colors.Red);
						}
						else
						{
                            RecColor_5 = new SolidColorBrush(Colors.Green);
						}
						DateTime dateTime = SelectedEntity.ValueList[4].Item1;
						Time_5 = dateTime.Minute.ToString() + ":" + dateTime.Second.ToString();
					}
					else
					{

                        RecColor_5 = new SolidColorBrush(Colors.Teal);
                        RecText_5 = "-";
						Time_5 = "00:00";
					}

				});

			}
			else if (Application.Current == null)
			{
				_timer.Dispose(); //if the application is shut down, dispose of the timer
			}
			else
			{
				//do nothing because no entity is selected
			}
		}
	}
}
