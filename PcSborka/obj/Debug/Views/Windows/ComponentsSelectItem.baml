   M S B A M L   `   `   `  ����C  ?PcSborka, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:PcSborka.Views.WindowsPcSborka.Views.Windows  5   	   1    +PcSborka.Views.Windows.ComponentsSelectItem   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6local$clr-namespace:PcSborka.Views.Windows     5   	     � Title$+  %Выбор комплектующих��5   	   $	��450��6%   $	��800��62   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��1* �6    � RowDefinitions 5   
   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��1* �6   .+�5   
   �� $	��650��6    �� TextAlignment$ Center=�6    $��20(�67   $��5q�6E   $��Italic$�6P   $
��Bold"�6c   $��2��6u   %#Меню комплектующих6�   �� 5   
   -   5         ��Name$ Back_button��5      $��12(�6'   $��5q�65   $��Italic$�6A   $
��Bold"�6T   $
��Left=�6f   $	��Top=�6�   $��
Назад��6�   6�   �� 5   
   -   5        $ ChooseCPU_button��5      $	��300��6,   $��1��68   $��12(�6E   $��5q�6S   $��Italic$�6_   $
��Bold"�6r   $	��Top=�6�   $��Процессор��6�    � Cursor$
 Hand��6�   6�   �� 5   
   -   5        $ ChooseMotherBoard_button��5      $	��300��64   $��2��6@   $��12(�6M   $��5q�6[   $��Italic$�6g   $
��Bold"�6z   $	��Top=�6�   $)��#Материнская память��6�   $
 Hand��6�   6�   �� 5   
   -   5        $ ChooseCase_button��5      $	��300��6,   $��3��68   $��12(�6E   $��5q�6S   $��Italic$�6_   $
��Bold"�6r   $	��Top=�6�   $��Корпус��6�   6�   �� 5   
   -   5        $ ChooseGPU_button��5      $	��300��6+   $��4��67   $��12(�6D   $��5q�6R   $��Italic$�6^   $
��Bold"�6q   $	��Top=�6�   $��Видеокарта��6�   6�   �� 5   
   -   5        $ ChooseCooler_button��5      $	��300��6.   $��5��6:   $��12(�6G   $��5q�6U   $��Italic$�6a   $
��Bold"�6t   $	��Top=�6�   $��Охлаждение��6�   6�   �� 5    
   -   5        $ ChooseRAM_button��5       $	��300��6+   $��6��67   $��12(�6D   $��5q�6R   $��Italic$�6^   $
��Bold"�6q   $	��Top=�6�   $)��#Оперативную память��6�   6�   �� 5!   
   -   5        $ ChoosePowerSupply_button��5!      $	��300��63   $��7��6?   $��12(�6L   $��5q�6Z   $��Italic$�6f   $
��Bold"�6y   $	��Top=�6�   $��Блок питания��6�   6�   5#      5$      