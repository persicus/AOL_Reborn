   M S B A M L   `   `   `  ����E  AAOL_Reborn, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:AOL_Reborn.ViewsAOL_Reborn.Views  5      %    AOL_Reborn.Views.ChatMainWindow   5      W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      0localclr-namespace:AOL_Reborn.Views     5        � Title$*  $SmarterChild - AOL Instant Messenger��5      $	��600��5      $	��550��5      ���#D4D0C8���#000080$��3q�5       � 
ResizeMode$ NoResize=�5       � WindowStartupLocation$ CenterScreen=�5       � 	Resources 5      4   .AOL_Reborn.Views.MessageSenderToColorConverter   SenderColorConverter��%   &	          5   
   6.   5      .��5      �  � RowDefinitions 5   
   �� 5      $��5* �6   �� 5      $
��Auto �6   �� 5      $
��1.5* �6   �� 5      $
��Auto �6   .+�5!   
   �� $��0��5"      $��1q�5#      ���#FFFFFF���#808080$��2q�5&      ._�5'      �� -   5         ��Name$ ChatScrollViewer��5'      $
��Auto=�65   .��5(      �� -   5         ��Name$ ChatMessages��5(      ��65   �� *
Messages+��5)      �� 5*      .R�5+      �� $��1q�5,   !    �� Padding$ 1q�5-   !   
������  �� CornerRadius$ 1��5/   !   ._�50   "   �� .+�52   &   �� $��Tahoma)�60   $��17(�6D   $
��Bold"�6R   ��53   0   �� *Sender+	 �� 	Converter#	 [  �� 54   *   �6.   �� *Sender+ �� 55   *   $� (��6.    �� 56   *   $��15(�6I   �6.   �� *	Timestamp+6I    �� 57   *   $�)��6.   58   '   �� 5:   &   $��Tahoma)�60   $��17(�6D   $
��Wrap=�5<   0   $��0,1,0,1q�6D   ��5;   0   �� *	Message+5<   D   5=   #   5>      5?      5A      5B      5C      �� 5F   
   $��1��5G      $��30��5H      ���#C0C0C0���#808080$��1q�5K      ._�5L      �� $��5q�5M      $
��Left=�5N      $��Center=�5O      
 �� Orientation$
 
Horizontal=�5P      .+�5Q      �� $��25��6   $��25��6$   ���#D4D0C8$��A��6E   �� 5R      $��25��6   $��25��6$   ���#D4D0C8$��B��6E   $
��Bold"�6Q   �� 5S      $��25��6   $��25��6$   ���#D4D0C8$��I��6E   $��Italic$�6Q   �� 5T      $��25��6   $��25��6$   ���#D4D0C8$
��😊��6E   5U      5V      �� 5Y   
   $��2��5Z      $ 5q�5[      ���#D4D0C8���#808080$��2q�5^      ._�5_      �  � ColumnDefinitions 5`      �� 5a      $��* �6'   �� 5b      $	��100 �6'   .+�5d      �� -   5         ��Name$ MessageInput��5d      $��0��5e      $��40��5f      $��5q�5g       �� AcceptsReturn$
 True��5h      $��Tahoma)�5i      $��17(�5j       �� TextWrapping$
 Wrap=�5k      " �� VerticalScrollBarVisibility$
 Auto=�5l      �� 5m      -   5        $��1��5m      $��90��5n      $��40��5o      ���#C0C0C0���#808080$��2q�5r      $
��Send��5t      $
��Bold"�5u      
��� � �5v      5w      5x      �� 5{   
   $��3��5|      $��40��5}      ���#C0C0C0���#808080$��1q�5�      ._�5�      �� $��5q�6   $
��Left=�6$   $��Center=�6?   $
 
Horizontal=�6Z   .+�5�      �� -   5        $	��100��5�      $��30��6%   $��10,0,0,0q�61   $
��Left=�5�      $��Center=�64   $��
Clear Chat��5�   9   5�      5�      5�      5�      