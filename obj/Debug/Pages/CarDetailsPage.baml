   M S B A M L   `   `   `  ����  CarShop    CarShopApp.CarDetailsPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        N� Title$1  +Подробности автомобиля��5      $	��600��6&   $
��1000��63   .��5      � ��5   
   �� 5       �� 
StartPoint$	 0,0&�6"    �� EndPoint$	 1,1&�63   .��5	      �  � Color$ 	#FF4A90E2��6    � Offset$ 0Q�61   � 5
      $ 	#FF2B5673��6   $ 1Q�61   5      .+�5   
   ��  �� Orientation$ Vertical=�6   $��Center=�6,   $	��Top=�6I   $��0,10,0,0q�6a   .+�5      �� $G��AДетальная информация об автомобиле��6   $��32(�6B   $
��Bold"�6P   $��Center=�6b   $��20q�6   5      �� 5   
   $
��Auto=�6   $��Disabled=�6:   $��10,100,10,10q�6c   .��5      �  � ColumnDefinitions 5      �� 5      $��2* �6'   �� 5      $��3* �6'   .+�5      �� -   5         ��Name$ CarImage��5      $	��226��6*   $��Center=�67   $	��Top=�6T   $��0,20,0,0q�6l   �� 5      $��1��6   $ Vertical=�6-   $
��Left=�6D   $	��Top=�6_   $��30,0,0,0q�6w   .+�5      �� -   5         ��Name$ CarModelTextBlock��5       $"��Модель: [Модель]��6;   $��18(�6S   $
��Bold"�6a   
�������$��0,5,0,5q�6�   �� 5      -   5        $ CarTypeTextBlock��5       $��Тип: [Тип]��6:   $��16(�6L   
�������$��0,5,0,5q�6m   �� 5      -   5        $ CarTransmissionTextBlock��5       $D��>Коробка передач: [Коробка передач]��6B   $��16(�6l   
�������$��0,5,0,5q�6�   �� 5       -   5        $ CarYearTextBlock��5        $%��Год выпуска: [Год]��6:   $��16(�6T   
�������$��0,5,0,5q�6u   �� 5!      -   5        $ CarMileageTextBlock��5!       $"��Пробег: [Пробег]��6=   $��16(�6U   
�������$��0,5,0,5q�6v   �� 5"      -   5        $ CarColorTextBlock��5"       $��Цвет: [Цвет]��6;   $��16(�6O   
�������$��0,5,0,5q�6p   �� 5#      -   5        $ CarEngineTypeTextBlock��5#       $"��Привод: [Привод]��6@   $��16(�6X   
�������$��0,5,0,5q�6y   �� 5$      -	   5        $ CarPriceTextBlock��5$       $��Цена: [Цена]��6;   $��16(�6O   $
��Bold"�6]   
�������$��0,5,0,15q�6�   5%      �� 5'      $ Vertical=�6   $
��Left=�64   $	��Top=�6O   $��
15,294,0,0q�6g   $��2��6{   $	��938��6�   .+�5(      �� -
   5        $ CarDescriptionTextBlock��5(       $?��9Описание: [Описание автомобиля]��6A   $��14(�6h   
�������6v   �� 5)      $4��.Конфигурация автомобиля:��6    $��18(�6@   $
��Bold"�6N   
�������$��0,10,0,5q�6s   �� 5*      -   5        $ CarSalonTextBlock��5*       $��Салон: [Салон]��6;   $��16(�6Q   
�������$��0,5,0,5q�6r   �� 5+      -   5        $ CarSafetySystemsTextBlock��5+       $X��RСистемы безопасности: [Системы безопасности]��6C   $��16(�6w   
�������$��0,5,0,5q�6�   �� 5,      -   5        $ CarAirbagsTextBlock��5,       $X��RПодушки безопасности: [Подушки безопасности]��6=   $��16(�6q   
�������$��0,5,0,5q�6�   �� 5-      -   5        $# CarAssistanceSystemsTextBlock��5-       $@��:Системы помощи: [Системы помощи]��6G   $��16(�6o   
�������$��0,5,0,5q�6�   �� 5.      -   5        $ CarExteriorTextBlock��5.       $.��(Экстерьер: [Экстерьер]��6>   $��16(�6\   
�������$��0,5,0,5q�6}   �� 5/      -   5        $ CarMultimediaTextBlock��5/       $6��0Мультимедиа: [Мультимедиа]��6@   $��16(�6b   
�������$��0,5,0,5q�6�   �� 50      $ 
Horizontal=�6!   $��8��6:   $
��Left=�6G   .+�51      �� -   5        $��Заказать��51   !   $	��150��6N   $��40��6Z   $��0,0,20,0q�6f   �� 52      -   5        $��Вернуться��52   !   $	��150��6N   $��40��6Z   $��0,0,20,0q�6g   �� 53      -   5        	 ��Name$	 DeleteCarButton��53   �   $��Удалить��6!   $	��150��63   $��40��6?   $y�	Collapsed=�6i   6�   55      56      59      5:      5;      5<      