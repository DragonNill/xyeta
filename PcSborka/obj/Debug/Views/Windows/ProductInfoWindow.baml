   M S B A M L   `   `   `  ����C  ?PcSborka, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:PcSborka.Views.WindowsPcSborka.Views.Windows  5   	   .    (PcSborka.Views.Windows.ProductInfoWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6local$clr-namespace:PcSborka.Views.Windows     5   	   -   5          � Title$=  7Подробная информация о товаре��5   	   $	��550��6/   $	��650��6<   $	��500��6H   $	��600��6X   .��5	      �� .��5
   
   �  � ColumnDefinitions 5      �� 5      $
��auto �6#   �� 5      $��1* �6#   �� 5      $
��auto �6#   .+�5      �� -   5         ��Name$ Back_button��5      $��12(�6*   $��5q�68   $��Italic$�6D   $
��Bold"�6W   $
��Left=�6i   $	��Top=�6�   $��
Назад��6�    � Cursor$
 Hand��6�   6�   � 5       � RowDefinitions 5      �� 5      �� 5      .+�5      �� -   5        $ AddToChoose_button��5      $��12(�65   $��25��6C   $��5q�6O   $��Italic$�6[   $
��Bold"�6n   $��Bottom=�6�   $
��Left=�6�   $��Добавить��6�   6�   �� 5      -   5        $ Update_button��5      $��12(�60   $��25��6>   $��5q�6J   $��Italic$�6V   $
��Bold"�6i   $	��Top=�6{   $��1��6�   $y�Hidden=�6�   $
��Left=�6�   $ ��Редактировать��6�   6�   5      � 5      $��1��6    5      �� 5      $��3* �6$   �� 5      $��2* �6$   �� 5      $��6* �6$   .+�5      �� -   5         ��Name$ Title_textBox��5      $��Bottom=�61   $��Center=�6L   $��16(�6i   $��5q�6w   $��Italic$�6�   $
��Bold"�6�   $
��Text��6�   {�.  6�   �� 5       -   5        $ Cost_textBox��5       $��Center=�60   $��Center=�6K   $��1��6h   $��12(�6u   $��5q�6�   $��Italic$�6�   $
��Bold"�6�   {�.  6%  �� 5!      -   5        $ Charastericts_textBox��5!      $
��Text��69    �� TextWrapping$
 Wrap=�6E   $	��Top=�6Y   $��Center=�6q   $��2��6�   $��12(�6�   $��5q�6�   $��Italic$�6�   $
��Bold"�6�   {�.  6�   5"      � 5#      $��2��6    5$      �� 5%      $��2* �6$   �� 5&      $��1* �6$   �� 5'      $��5* �6$   .+�5)      �� -   5        $ Socket_textBox��5)       �� TextAlignment$
 Left=�62   $��Bottom=�6G   $
��Left=�6b   $��12(�6}   $��5q�6�   $��Italic$�6�   $
��Bold"�6�   $y�Hidden=�6�   {�.  6�   �� 5*      -	   5        $ TypeMemory_textBox��5*      $
 Left=�66   $��Center=�6K   $
��Left=�6f   $��1��6�   $��12(�6�   $��5q�6�   $��Italic$�6�   $
��Bold"�6�   $y�Hidden=�6�   {�.  6�   �� 5+      -
   5        $ Form_Factor_textBox��5+      $
 Left=�67   $	��Top=�6L   $
��Left=�6d   $��2��6   $��12(�6�   $��5q�6�   $��Italic$�6�   $
��Bold"�6�   $y�Hidden=�6�   {�.  6�   5,      5-      5.      5/      