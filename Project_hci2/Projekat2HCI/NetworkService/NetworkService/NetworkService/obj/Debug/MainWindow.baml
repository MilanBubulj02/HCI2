   M S B A M L   `   `   `  ����c  _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity  5   	   I ENetworkService, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:NetworkServiceNetworkService 5   	   ?$clr-namespace:NetworkService.HelpersNetworkService.Helpers 5   	   C&clr-namespace:NetworkService.ViewModelNetworkService.ViewModel 5	   	   ;"clr-namespace:NetworkService.ViewsNetworkService.Views 5
   	      NetworkService.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     5   	   .localclr-namespace:NetworkService     5   	   9terminal$clr-namespace:NetworkService.Helpers     5   	   <	viewModel&clr-namespace:NetworkService.ViewModel     5	   	   4views"clr-namespace:NetworkService.Views     5
   	     � 
ResizeMode$  CanMinimize=�5   	    � Title$ 
MainWindow��5   	   $	��800��6   $
��1420��6)    � DataContext 5      2  ,NetworkService.ViewModel.MainWindowViewModel  5   
    � 	Resources 5       �� DataType0  *NetworkService.ViewModel.EntitiesViewModel'  !NetworkService.Views.EntitiesView/  )NetworkService.ViewModel.DisplayViewModel&   NetworkService.Views.DisplayView-  'NetworkService.ViewModel.GraphViewModel$  NetworkService.Views.GraphView��%�   (��       5      *, +)(�� 2     5      *, +)(�� d     5      *, +)�� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5        5      5       J� InputBindings 5      �� 5   
   
 �� Key$
 Oem3��5        �� 	Modifiers$ Shiftf�5!       �� Command 5      �� *ShiftBacktickCommand+5!      �� 5"   
   $ D1��5#      $ Shiftf�5$       5"      �� *Shift1_Command+5$      �� 5%   
   $ D2��5&      $ Shiftf�5'       5%      �� *Shift2_Command+5'      �� 5(   
   $ D3��5)      $ Shiftf�5*       5(      �� *Shift3_Command+5*      .��5-      � 	 � ColumnDefinitions	 5.   
   �� 5/      $	��100 �6   �� 50      .+�54   
   �� $	��100��6   $��Center=�6    
�������6=   �� 57   
   -   5        
 ��Name$
 
UndoButton��58      $	��Top=�59       � Cursor$
 Hand��5:      $��0,20,0,0q�5;      $
��back��5=         SidebarButtonStyle#��[  5>      ��5<      �� *
NavCommand+��5?      �� 5@       �� ImageSource$ Assets/undo.png��6    j� Stretch$ Uniform=�6;   5C      �� 5E   
   -   5        $
 EntitiesViewButton��5F      $��Center=�5G      $��	20,0,20,0q�5H      $
 Hand��5I      $��entities��5K      #��[  5L      ��5J      �� *
NavCommand+��5M      �� 5N      $ Assets/add-entity.png��6   $ Uniform=�6A   5P      �� 5R   
   -   5        $
 DisplayViewButton��5S      $	��Top=�5T      $��20,185,20,0q�5U      $
 Hand��5V      $��display��5X      #��[  5Y      ��5W      �� *
NavCommand+��5Z      �� 5[      $ Assets/Home.png��6   $ Uniform=�6;   5]      �� 5_   
   -   5        $
 GraphViewButton��5`      $	��Top=�5a      $��20,535,20,0q�5b      $
 Hand��5c      $��graph��5e      #��[  5f      ��5d      �� *
NavCommand+��5g      �� 5h      $ Assets/stats.png��6   $ Uniform=�6<   5j      �� 5l   
   -   5        $
 TerminalButton��5m      $��Bottom=�5n      $��0,0,0,20q�5o      $
 Hand��5p      #��[  5r      ��5q      �� *TerminalCommand+��5s      �� 5t      $  Assets/square-terminal.png��6   $ Uniform=�6F   5v      �� 5x   
   $��1��5y      ��5z      �� *CurrentViewModel+�� 5}   
   -   5         ��Name$ TerminalDisplayTextBox��5}      " �� VerticalScrollBarVisibility$
 Auto=�5~      $	��200��5      $��Bottom=�5�      $��0 0 0 30q�5�      $��1��5�       �� TextWrapping$
 Wrap=�5�       �� 
IsReadOnly . 
���   ����#000edf$��Consolas)�5�      $��14(�5�      y�5�      �� *TerminalDisplayVisible+��5�      �� *TerminalDisplay+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� 5�   
   -   5        $ TerminalInputTextBox��5�      
���   ����#000edf$��1��5�      $��Consolas)�5�      $��14(�5�      $��Bottom=�5�      $��30��5�      $��	140,0,0,0q�5�      y�5�      �� *TerminalInputVisible+��5�      �� *TerminalInput+$ TwoWay=�$ PropertyChanged=�.��5�   %   .   (System.Windows.Interactivity.Interaction   	Behaviors 5�      -	  'NetworkService.Helpers.KeyPressBehavior	  5�       	  Command 6,   �� *KeyPressedCommand+.��5�   '   5�      �� 5�   
   -   5         ��Name$ TerminalLabel��5�      $��1��5�      $��14(�5�      $��Consolas)�5�      $��30��5�      $	��140��5�      $
��Left=�5�      $��Bottom=�5�      
���   ����#000edf��5�      �� *TerminalLabel+y�5�      �� *TerminalLabelVisible+5�      5�      