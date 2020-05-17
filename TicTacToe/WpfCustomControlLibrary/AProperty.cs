using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WpfCustomControlLibrary
{
	public static class AProperty
	{
		/// <summary>Получить количество строк Grid</summary>
		/// <param name="grid">Grid к которому присоединено свойство</param>
		/// <returns>Возвращает заданное количество строк</returns>
		/// <remarks>Если после задания свойства,
		/// количество строк в Grid менялось иным способом,
		/// то значение AP-свойства будет отличаться от 
		/// реально имеющегося количества строк в Grid</remarks>
		public static int GetRows(Grid grid)
		{
			return (int)grid.GetValue(RowsProperty);
		}

		/// <summary>Получить количество строк Grid</summary>
		/// <param name="grid">Grid к которому присоединено свойство</param>
		/// <param name="rows">Задаваемое количество строк</param>
		public static void SetRows(Grid grid, int rows)
		{
			grid.SetValue(RowsProperty, rows);
		}

		// Using a DependencyProperty as the backing store for Rows.  This enables animation, styling, binding, etc...
		/// <summary>AP-свойство для задания количества строк в Grid</summary>
		/// <remarks>Если после задания свойства,
		/// количество строк в Grid менялось иным способом,
		/// то значение AP-свойства будет отличаться от 
		/// реально имеющегося количества строк в Grid</remarks>
		public static readonly DependencyProperty RowsProperty =
				DependencyProperty.RegisterAttached("Rows", typeof(int), typeof(AProperty), new PropertyMetadata(0, RowsChanged));

		/// <summary>Метод обратного вызова при изменении значения свойства RowsProperty</summary>
		/// <param name="d">Объект в котором было изменено свойство</param>
		/// <param name="e">Параметры изменения</param>
		private static void RowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(d is Grid grid))
				throw new ArgumentException("Must be a Grid", nameof(d));

			grid.RowDefinitions.Clear();
			int rows = (int)e.NewValue;
			for (int i = 0; i < rows; i++)
			{
				grid.RowDefinitions.Add(new RowDefinition());
			}
		}



		/// <summary>Получить количество колонок Grid</summary>
		/// <param name="grid">Grid к которому присоединено свойство</param>
		/// <returns>Возвращает заданное количество колонок</returns>
		/// <remarks>Если после задания свойства,
		/// количество колонок в Grid менялось иным способом,
		/// то значение AP-свойства будет отличаться от 
		/// реально имеющегося количества колонок в Grid</remarks>
		public static int GetColumns(Grid grid)
		{
			return (int)grid.GetValue(ColumnsProperty);
		}

		/// <summary>Получить количество колонок Grid</summary>
		/// <param name="grid">Grid к которому присоединено свойство</param>
		/// <param name="rows">Задаваемое количество колонок</param>
		public static void SetColumns(Grid grid, int rows)
		{
			grid.SetValue(ColumnsProperty, rows);
		}

		// Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
		/// <summary>AP-свойство для задания количества колонок в Grid</summary>
		/// <remarks>Если после задания свойства,
		/// количество колонок в Grid менялось иным способом,
		/// то значение AP-свойства будет отличаться от 
		/// реально имеющегося количества колонок в Grid</remarks>
		public static readonly DependencyProperty ColumnsProperty =
			DependencyProperty.RegisterAttached("Columns", typeof(int), typeof(AProperty), new PropertyMetadata(0, ColumnsChanged));

		/// <summary>Метод обратного вызова при изменении значения свойства ColumnsProperty</summary>
		/// <param name="d">Объект в котором было изменено свойство</param>
		/// <param name="e">Параметры изменения</param>
		private static void ColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(d is Grid grid))
				throw new ArgumentException("Must be a Grid", nameof(d));

			int columns = (int)e.NewValue;
			grid.ColumnDefinitions.Clear();
			for (int i = 0; i < columns; i++)
			{
				grid.ColumnDefinitions.Add(new ColumnDefinition());
			}
		}


		/// <summary>Получение заданной пропорции отношения ширины к высоте элемента.
		/// Задаётся коэффициент для получения размера высоты из ширины: Height = Width * WidthToHeight</summary>
		/// <param name="element">FrameworkElement чьи размеры должны быть пропорциональны</param>
		public static double GetWidthToHeight(FrameworkElement element)
		{
			return (double)element.GetValue(WidthToHeightProperty);
		}

		/// <summary>Задание пропорции отношения ширины к высоте элемента.
		/// Задаётся коэффициент для получения размера высоты из ширины: Height = Width * WidthToHeight</summary>
		/// <param name="element">FrameworkElement чьи размеры должны быть пропорциональны</param>
		/// <param name="widthToHeight">Коэфициент пропорции:  Height = Width * WidthToHeight</param>
		/// <returns>Отслеживается изменение размера контейнера предоставленного элемента.
		/// При его изменении определяется максимально возможный размер элемента который может
		/// быть достигнут в контейнере при соблюдении заданных пропорций</returns>
		public static void SetWidthToHeight(FrameworkElement element, double widthToHeight)
		{
			element.SetValue(WidthToHeightProperty, widthToHeight);
		}

		// Using a DependencyProperty as the backing store for Proportionate.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty WidthToHeightProperty =
			DependencyProperty.RegisterAttached("WidthToHeight", typeof(double), typeof(AProperty), new PropertyMetadata(-1.0, ProportionalChanged));

		/// <summary>Метод обратного вызова после изменения значения свойства.</summary>
		/// <param name="d">FrameworkElement иначе исключение.</param>
		/// <param name="e">Параметры изменения</param>
		private static void ProportionalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(d is FrameworkElement element))
				throw new ArgumentException("Must be a FrameworkElement");

			/// Получение элемента сохраняющего пропорции.
			SizeChangedElement changedElement = GetSizeChangeElement(element);

			/// Если элемент не задан, то его создание и сохранение.
			if (changedElement == null)
				SetSizeChangeElement(element, changedElement = new SizeChangedElement(element));

			/// Передача нового значения пропорции.
			changedElement.SetWidthToHeight((double)e.NewValue);
		}




		private static SizeChangedElement GetSizeChangeElement(DependencyObject obj)
		{
			return (SizeChangedElement)obj.GetValue(SizeChangeElementPropertyKey.DependencyProperty);
		}

		private static void SetSizeChangeElement(DependencyObject obj, SizeChangedElement value)
		{
			obj.SetValue(SizeChangeElementPropertyKey, value);
		}

		//// Using a DependencyProperty as the backing store for SizeChangeElement.  This enables animation, styling, binding, etc...
		//private static readonly DependencyProperty SizeChangeElementProperty =
		//	DependencyProperty.RegisterAttached("SizeChangeElement", typeof(SizeChangedElement), typeof(AProperty), new PropertyMetadata(null));

		// Using a DependencyProperty as the backing store for SizeChangeElement.  This enables animation, styling, binding, etc...
		/// <summary>Регистрация AP-свойства только для чтения</summary>
		private static readonly DependencyPropertyKey SizeChangeElementPropertyKey =
			DependencyProperty.RegisterAttachedReadOnly("SizeChangeElement", typeof(SizeChangedElement), typeof(AProperty), new PropertyMetadata(null));

		/// <summary>Вспомогательный класс отслеживающий изменения размера элемента</summary>
		private class SizeChangedElement
		{
			/// <summary>FrameworkElement. Не может быть null.</summary>
			public FrameworkElement Element { get; }

			/// <summary>Задётся коэфициент для получения размера высоты из ширины: Height = Width * WidthToHeight.
			/// Если равен или меньше нуля, то соблюдение пропорции не производится.</summary>
			public double WidthToHeight { get; private set; } = 1;

			/// <summary>Конструктор принимающий элемент чьи пропорции не должны меняться.</summary>
			/// <param name="element">FrameworkElement. Не может быть null.</param>
			public SizeChangedElement(FrameworkElement element)
			{
				Element = element ?? throw new ArgumentNullException(nameof(element));

				/// Событие происходит при изменении выделяемого для элемента места
				element.LayoutUpdated += Element_LayoutUpdated;
			}

			/// <summary>Обработчик вызываемый при изменении выделяемого для элемента места.</summary>
			/// <param name="sender"><see langword="null"/>.</param>
			/// <param name="e">Empty or <see langword="null"/>.</param>
			private void Element_LayoutUpdated(object sender = null, EventArgs e = null)
			{
				if (WidthToHeight <= 0)
					return;

				/// Получение информации о выделенном месте
				Rect rect = LayoutInformation.GetLayoutSlot(Element);

				/// Размеры выделенной области с учётом Margin элемента
				double widthArea = rect.Width - Element.Margin.Left - Element.Margin.Right;
				double heightArea = rect.Height - Element.Margin.Top - Element.Margin.Bottom;

				double width = widthArea;
				double height = width * WidthToHeight;
				if (height > heightArea)
				{
					height = heightArea;
					width = height / WidthToHeight;
				}
				Element.Width = width > 0.0 ? width : 0.0;
				Element.Height = height > 0.0 ? height : 0.0;
			}

			/// <summary>Задание коэффициента пропорции ширины к высоте.</summary>
			/// <param name="widthToHeight">Коэффициент пропорции ширины к высоте: Height = Width * WidthToHeight.</param>
			public void SetWidthToHeight(double widthToHeight)
			{
				WidthToHeight = widthToHeight;
				Element_LayoutUpdated();
			}

		}

	}
}
