   M S B A M L   `   `   `  ���� H  DAvalancheDemo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:AvalancheDemoAvalancheDemo  [ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms C&clr-namespace:AvalancheDemo.PropertiesAvalancheDemo.Properties      AvalancheDemo.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:AvalancheDemo     Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     7resx&clr-namespace:AvalancheDemo.Properties     -     �Name$  
thisWindow�� � Title$ Notit��$	��500��$��838.746�� � 
ResizeMode$ NoResize=�$	��600��$	��500�� � SizeToContent$ WidthAndHeight=� � Icon$6 0pack://siteoforigin:,,,/Resources/Logo T (1).png��.��� $��	0,1,0,1.2q�$	��698��$
��Left=� � ColumnDefinitions �� �� $��2* ��� $��1* � � RowDefinitions �� $
��Auto ��� �� $
��Auto �.+��� $��60�� �� Orientation$ 
Horizontal=����	#FF03A9F4.+�A� $��10,10,10,10q�$��40��
����������� *Height+ �� RelativeSource �� *Self+�� -   	 ��Name$	 textBox��
 �� TextWrapping$

 Wrap=�$��94��$
��0,20q�$��Center=�
����������� *
UserName+ �� ElementName$ 
thisWindow����Z� �� -   $��Local Preview��$��1��$��10,10,10.4,0q�$	��Top=����	#FF212121� $��2�����	#FF0288D1 �� �� .+��� -   $
��Join��$
��Left=�$��0q�
����������	#00000000$��25��$��Center=���Z� �� $	��New��$��Right=�
����������	#00000000$��1��$��25��$	��Top=���Z� � -    �Name$ 	videoCell��$��1��$��3�� �� �� $
��Auto �.+�� -   $ remoteVideoPanel�� �� $	��200 ��� $	��200 � �� �� .+�7  1System.Windows.Forms.Integration.WindowsFormsHost     
Foreground Z�    Child. %  System.Windows.Forms.PictureBox  -   pictureBox11��  $��1�� Z� .   -   pictureBox12��  $��1�� Z� .   -	   pictureBox21��  $��1��$��1�� Z� .   -
   pictureBox22��     
Background
 �����$��60��$��80��$��	0,10,10,0q�$��Right=�$	��Top=� Z� .   -   
videoLocal���� -    ��Name$ 	endButton��$��Center=�$��Bottom=�$��45��$��152,0,151.8,-0.2q�$��1��}�. $��45������  �� ImageSource$0 *pack://siteoforigin:,,,/Resources/Stop.png�� j� Stretch$ Uniform=��� -    ��Name$ listBox��$��1��$��348.6,0,0,9.8q�$��3���� -   $ channelUserListBox��$
��Left=�$	��319��$��	10,30,0,0q�$��1��$	��Top=�$	��154�����	#FF212121