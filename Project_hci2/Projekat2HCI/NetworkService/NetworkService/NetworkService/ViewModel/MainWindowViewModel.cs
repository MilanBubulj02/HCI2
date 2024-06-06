﻿using MVVMLight.Messaging;
using NetworkService.Helpers;
using NetworkService.Model;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using NetworkService.Model;
using System.IO;

namespace NetworkService.ViewModel
{
    public class MainWindowViewModel : BindableBase
	{

		private string terminalDisplayVisible;
		private string terminalInputVisible;
		private string terminalLabelVisible;
		private static bool terminalVisible = false;
		private bool idFound = false;

		public MyICommand<string> NavCommand { get; private set; }
        public MyICommand<Window> CloseWindowCommand { get; private set; }
        public static ObservableCollection<Entity> FileImport { get; set; }
        public static ObservableCollection<Entity> Entities { get; set; } = new ObservableCollection<Entity>();
		public static ObservableCollection<Entity> Interval_Entities { get; set; } = new ObservableCollection<Entity>();
		public static ObservableCollection<Entity> Smart_Entities { get; set; } = new ObservableCollection<Entity>();
        public EntitiesViewModel entitiesViewModel;
        public DisplayViewModel displayViewModel;
        public GraphViewModel graphViewModel;
        private BindableBase currentViewModel;
		private NotificationManager notificationManager;
        List<string> BackList = new List<string>();

        public ICommand KeyPressedCommand { get; }
		public ICommand ShiftBacktickCommand { get; }
		public ICommand Shift1_Command { get; }
		public ICommand Shift2_Command { get; }
		public ICommand Shift3_Command { get; }
		public ICommand TerminalCommand { get; private set; }

		public string TerminalDisplayVisible
		{
			get { return terminalDisplayVisible; }
			set
			{
				if (terminalDisplayVisible != value)
				{
					terminalDisplayVisible = value;
					OnPropertyChanged(nameof(TerminalDisplayVisible));
				}
			}
		}

		public string TerminalInputVisible
		{
			get { return terminalInputVisible; }
			set
			{
				if (terminalInputVisible != value)
				{
					terminalInputVisible = value;
					OnPropertyChanged(nameof(TerminalInputVisible));
				}
			}
		}

		public string TerminalLabelVisible
		{
			get { return terminalLabelVisible; }
			set
			{
				if (terminalLabelVisible != value)
				{
					terminalLabelVisible = value;
					OnPropertyChanged(nameof(TerminalLabelVisible));
				}
			}
		}

		private string terminalInput;
		public string TerminalInput
		{
			get { return terminalInput; }
			set 
			{ 
				terminalInput = value; 
				OnPropertyChanged(nameof(TerminalInput));
			}
		}
		private string terminalDisplay;
		public string TerminalDisplay
		{
			get { return terminalDisplay; }
			set { terminalDisplay = value; OnPropertyChanged(nameof(TerminalDisplay)); }
		}

		private string terminalLabel;
		public string TerminalLabel
		{
			get { return terminalLabel; }
			set { terminalLabel = value; OnPropertyChanged(nameof(TerminalLabel)); }
		}
		public MainWindowViewModel()
        {
            createListener(); //Povezivanje sa serverskom aplikacijom

            FileImport = XmlHelper.LoadData("entityData.xml");

            notificationManager = new NotificationManager();

			NavCommand = new MyICommand<string>(OnNav);
			TerminalCommand = new RelayCommand(OnTerminalButton);
			CloseWindowCommand = new MyICommand<Window>(CloseWindow);
			ShiftBacktickCommand = new RelayCommand(OnShiftBacktickPressed);
			Shift1_Command = new RelayCommand(OnShift1);
			Shift2_Command = new RelayCommand(OnShift2);
			Shift3_Command = new RelayCommand(OnShift3);

			Entities = new ObservableCollection<Entity>();

			entitiesViewModel = new EntitiesViewModel();
            displayViewModel = new DisplayViewModel();
            graphViewModel = new GraphViewModel();
            CurrentViewModel = displayViewModel;

			KeyPressedCommand = new RelayCommand(OnKeyPressed);
			TerminalInput = string.Empty;
			TerminalDisplay = string.Empty;
			TerminalLabel = "Terminal input: ";
			ShowHide_Terminal();
		}

		private void OnTerminalButton(object parameter)
		{
			terminalVisible = !terminalVisible;
			ShowHide_Terminal();
		}

		private void ShowHide_Terminal()
		{
			if (terminalVisible == true)
			{
				TerminalDisplayVisible = "Hidden";
				TerminalInputVisible = "Hidden";
				TerminalLabelVisible = "Hidden";
			}
			else
			{
				TerminalDisplayVisible = "Visible";
				TerminalInputVisible = "Visible";
				TerminalLabelVisible = "Visible";
			}

		}

		private void OnShift1(object parameter)
		{
			OnNav("display");
		}
		private void OnShift2(object parameter)
		{
			OnNav("entities");
		}
		private void OnShift3(object parameter)
		{
			OnNav("graph");
		}

		private void OnShiftBacktickPressed(object parameter)
		{
			terminalVisible = !terminalVisible;
			ShowHide_Terminal();
		}

		private void OnKeyPressed(object parameter)
		{
			if (parameter is KeyEventArgs e)
			{
				if (e.Key == Key.Escape)
					TerminalInput = string.Empty;
				if (e.Key == Key.Tab)
				{
					e.Handled = true;
					AutoCompleteCommands();
				}


				if (e.Key == Key.Enter)
				{
					TerminalDisplay += '\n' + TerminalInput;

					if (TerminalLabel == "Entity input: ")
					{
						HandleAddEntity();
					}

					if (TerminalInput == string.Empty)
						return;

					if (TerminalInput.ToLower().StartsWith("add"))
					{
						HandleAddEntity();
					}
					else if (TerminalInput.ToLower().StartsWith("del"))
					{
						HandleDeleteEntity();
					}
					else if (TerminalInput.ToLower().StartsWith("back"))
					{
						HandleLastView();

                    }
					TerminalInput = string.Empty;
				}
			}
		}

		private void AutoCompleteCommands()
		{
			if(IsSubsequence(TerminalInput, "add"))
				TerminalInput = "add";
			else if (IsSubsequence(TerminalInput, "del"))
				TerminalInput = "del";
			else if (IsSubsequence(TerminalInput, "back"))
				TerminalInput = "back";

		}

		private static bool IsSubsequence(string input, string target)
		{
			int inputIndex = 0;
			int targetIndex = 0;

			while (inputIndex < input.Length && targetIndex < target.Length)
			{
				if (input[inputIndex] == target[targetIndex])
				{
					inputIndex++;
				}
				targetIndex++;
			}

			return inputIndex == input.Length;
		}

	private void HandleAddEntity()
		{
			string[] parameters = TerminalInput.Split(' ');
			if (parameters.Length != 4)
			{
				TerminalDisplay += '\n' + "Input error, correct syntax is: add (id: int) (name: string) (type: IntervalMeter or SmartMeter)";
				TerminalInput = string.Empty;
			}
			else if (parameters[3].ToLower() != "intervalmeter" && parameters[3].ToLower() != "smartmeter")
			{
				TerminalDisplay += '\n' + "Add Entity Error: types are IntervalMeter and SmartMeter";
				TerminalInput = string.Empty;
			}
			else
			{
				entitiesViewModel.IdText = parameters[1];
				entitiesViewModel.NameText = parameters[2];
				if (parameters[3].ToLower() == "intervalmeter")
				{
					entitiesViewModel.IntervalChecked = true;
					entitiesViewModel.SmartChecked = false;
				}
				else
				{
					entitiesViewModel.IntervalChecked = false;
					entitiesViewModel.SmartChecked = true;
				}

				entitiesViewModel.OnAdd();

				TerminalDisplay += '\n' + "Entity added successfully";
			}
		}

		private void HandleDeleteEntity()
		{
			string[] parameters = TerminalInput.Split(' ');
			idFound = false;
			foreach (string id in parameters.Skip(1))
			{
				if (!int.TryParse(id, out int result))
				{
					TerminalDisplay += '\n' + "Input error, correct syntax is: del (id: int)";
					TerminalInput = string.Empty;
				}
				else
				{
					foreach (var item in entitiesViewModel._Entities.Where(e => e.Id == result).ToList())
					{
						item.IsSelected = true;
						idFound = true;
					}

					if (idFound)
					{
						entitiesViewModel.OnDelete();
						TerminalDisplay += '\n' + $"Entity with id {result} deleted successfully.";
						TerminalInput = string.Empty;
					}
					else
					{
						TerminalDisplay += '\n' + $"Error: Entity with id {result} not found.";
					}
				}
			}
		}

		private void HandleSearchEntity()
		{
			string[] parameters = TerminalInput.Split(' ');
			if (parameters.Length != 3)
			{
				TerminalDisplay += '\n' + "Input error, correct syntax is: search (option: string) (query: string)";
				TerminalInput = string.Empty;
			}
			if (parameters[1].ToLower() == "naziv" || parameters[1].ToLower() == "name")
			{

			}
			else if (parameters[1].ToLower() == "tip" || parameters[1].ToLower() == "type")
			{

			}
			else
			{
				TerminalDisplay += '\n' + "Input error, avaivable options are: naziv/name and tip/type";
				TerminalInput = string.Empty;
			}
		}

		private void createListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25675);
            tcp.Start();

            var listeningThread = new Thread(() =>
            {
                while (true)
                {
                    var tcpClient = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(param =>
                    {
                        //Prijem poruke
                        NetworkStream stream = tcpClient.GetStream();
                        string incomming;
                        byte[] bytes = new byte[1024];
                        int i = stream.Read(bytes, 0, bytes.Length);
                        //Primljena poruka je sacuvana u incomming stringu
                        incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        //Ukoliko je primljena poruka pitanje koliko objekata ima u sistemu -> odgovor
                        if (incomming.Equals("Need object count"))
                        {
                            //Response
                            /* Umesto sto se ovde salje count.ToString(), potrebno je poslati 
                             * duzinu liste koja sadrzi sve objekte pod monitoringom, odnosno
                             * njihov ukupan broj (NE BROJATI OD NULE, VEC POSLATI UKUPAN BROJ)
                             * */
                            //Byte[] data = System.Text.Encoding.ASCII.GetBytes(count.ToString());
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(Entities.Count.ToString());
                            stream.Write(data, 0, data.Length);
                        }
                        else
                        {
                            //U suprotnom, server je poslao promenu stanja nekog objekta u sistemu
                            Console.WriteLine(incomming); //Na primer: "Entitet_1:272"

                            //################ IMPLEMENTACIJA ####################
                            // Obraditi poruku kako bi se dobile informacije o izmeni
                            // Azuriranje potrebnih stvari u aplikaciji

                            if (Entities.Count > 0)
							{
								var splited = incomming.Split(':');
								DateTime dt = DateTime.Now;
								using (StreamWriter sw = File.AppendText("Log.txt"))
								sw.WriteLine(dt + "; " + splited[0] + ", " + splited[1]);

								int id = Int32.Parse(splited[0].Split('_')[1]);

								Entities[id].Value = Math.Round(Double.Parse(splited[1]), 2);
								AddValueToList(Entities[id]);
							}
						}
                    }, null);
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }


		private void AddValueToList(Entity entity)
		{
			if (entity.ValueList.Count == 5)
			{
				entity.ValueList.RemoveAt(0);
				entity.ValueList.Add(new Pair<DateTime, double>(DateTime.Now, entity.Value));
			}
			else
			{
				entity.ValueList.Add(new Pair<DateTime, double>(DateTime.Now, entity.Value));
			}
		}

		public BindableBase CurrentViewModel
		{
			get { return currentViewModel; }
			set
			{
				SetProperty(ref currentViewModel, value);
			}
		}
		private void OnNav(string destination)
		{
			switch (destination)
			{
				case "entities":
					BackList.Add("entities");
					CurrentViewModel = entitiesViewModel;
					break;
				case "display":
                    BackList.Add("display");
                    CurrentViewModel = displayViewModel;
					break;
				case "graph":
                    BackList.Add("graph");
                    CurrentViewModel = graphViewModel;
					break;
				case "back":
					HandleLastView();
                    break;
			}
		}

        private void HandleLastView()
        {
            if (BackList.Count == 0)
            {
                CurrentViewModel = displayViewModel;
                return;
            }

            var destination = BackList[BackList.Count - 1];
            BackList.RemoveAt(BackList.Count - 1);

            switch (destination)
            {
                case "entities":
                    CurrentViewModel = entitiesViewModel;
                    break;
                case "display":
                    CurrentViewModel = displayViewModel;
                    break;
                case "graph":
                    CurrentViewModel = graphViewModel;
                    break;
            }

        }

        private void ShowToastNotification(NotificationContent notificationContent)
		{
			notificationManager.Show(notificationContent, "WindowNotificationArea");
		}

		private void CloseWindow(Window MainWindow)
		{
			MainWindow.Close();
		}

		public void UpdateCanvasValue(int id)
		{
			foreach (var tmp in DisplayViewModel.CanvasIDCollection)
			{
				int canvasId = entitiesViewModel.GetCanvasIndexForEntityId(id);
				if (tmp.Equals(canvasId.ToString()))
				{
					try
					{
						DisplayViewModel.CanvasValueCollection[canvasId] = entitiesViewModel._Entities[id].Value.ToString();
					}
					catch (Exception ex) { }
				}
			}
		}
	}
}
