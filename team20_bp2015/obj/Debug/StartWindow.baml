   M S B A M L   `   `   `  ����H  Dteam20_bp2015, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:team20_bp2015team20_bp2015  5   	       team20_bp2015.StartWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:team20_bp2015     5   	     � Title$  StartWindow��5   	   $	��417��6   $	��300��6*   .��5	      � .+�5
   
   �� -   5         ��Name$ button_server��5
      $��
I am Host.��6(   $
��Left=�6=   $	��Top=�6X   $	��115��6p   $��
10,160,0,0q�6|   $��50��6�   6�   �� 5   
   -   5        $ button_client��5      $��I am Client.��6(   $
��Left=�6?   $	��Top=�6Z   $	��112��6r   $��170,160,0,0q�6~   $��50��6�   6�   �� 5   
   -   5        $ button_Start��5      $��Start��6'   $
��Left=�67   $	��Top=�6R   $	��272��6j   $��
10,322,0,0q�6v   $��54��6�   6�   �� 5   
   -   5         ��Name$ textBoxIpInput��5      $
��Left=�6*   $��35��6E    �� TextWrapping$
 Wrap=�6Q   $��0.0.0.0��6e   $	��Top=�6t   $	��272��6�   $��	10,45,0,0q�6�   6�   �� 5   
   -   5         ��Name$ 	textBlock��5      $
��Left=�6'   $
��Wrap=�6B   $	��Top=�6V   $��	10,10,0,0q�6n   $��30��6�   $	��272��6�   �� 6�   
�������
����  �$��12.6666666666667(�6�   $��Consolas)�6�   $0�*Type the Host IP in the following TextBox.��6�   6*  �� 5   
   -   5        $ 
textBlock1��5      $
��Left=�6(   $
��Wrap=�6C   $	��Top=�6W   $��	10,85,0,0q�6o   $��30��6�   $	��155��6�   �� 6�   $�Type the Port:��6�   6�   �� 5   
   -   5        $ textBox��5      $
��Left=�6#   $��30��6>   $
 Wrap=�6J   $
��8080��6^   $	��Top=�6j   $	��112��6�   $��
170,85,0,0q�6�   6�   �� 5   
   -   5        $ settings��5      $��Settings...��6#   $
��Left=�69   $	��Top=�6T   $	��110��6l   $��170,225,0,0q�6x    J� RenderTransformOrigin$ 0.5,0.5&�6�   $��35��6�   z�5      V� 5      .~�5      �� �� 5      �� 5       �� Angle$ 0.825Q�6&   T� 5      5      5      �� 5   
   -	   5        $ button��5      $��Check��6!   $
��Left=�61   $	��Top=�6L   $��52��6d   $��230,269,0,0q�6o   $��25��6�   6�   �� 5   
   -
   5        $ 
textBlock2��5      $
��Left=�6(   $
��Wrap=�6C   $-��'All settings-values have to be numbers!��6W   $	��Top=�6�   $��
10,269,0,0q�6�   $��21��6�   $	��215��6�   �� 5   
   -   5        $ button_init��5      $��Initialize Corners...��6&   $
��Left=�6F   $	��Top=�6a   $	��270��6y   $��
12,120,0,0q�6�   6�   5      5       