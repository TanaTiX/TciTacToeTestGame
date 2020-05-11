using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace View
{
	public static class AProperty
	{


		public static int GetRows(Grid obj)
		{
			return (int)obj.GetValue(RowsProperty);
		}

		public static void SetRows(Grid obj, int value)
		{
			obj.SetValue(RowsProperty, value);
		}

		// Using a DependencyProperty as the backing store for Rows.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty RowsProperty =
			DependencyProperty.RegisterAttached("Rows", typeof(int), typeof(AProperty), new PropertyMetadata(0, RowsChanged));

		private static void RowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is Grid grid)
			{
				grid.RowDefinitions.Clear();
				int rows = (int)e.NewValue;
				for (int i = 0; i < rows; i++)
				{
					grid.RowDefinitions.Add(new RowDefinition());
				}
			}
			else
			{
				throw new ArgumentException("Не верный тип", nameof(d));
			}
		}



		public static int GetColumns(Grid obj)
		{
			return (int)obj.GetValue(ColumnsProperty);
		}

		public static void SetColumns(Grid obj, int value)
		{
			obj.SetValue(ColumnsProperty, value);
		}

		// Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ColumnsProperty =
			DependencyProperty.RegisterAttached("Columns", typeof(int), typeof(AProperty), new PropertyMetadata(0, ColumnsChanged));

		private static void ColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is Grid grid)
			{
				int columns = (int)e.NewValue;
				grid.ColumnDefinitions.Clear();
				for (int i = 0; i < columns; i++)
				{
					grid.ColumnDefinitions.Add(new ColumnDefinition());
				}
			}
			else
			{
				throw new ArgumentException("Не верный тип", nameof(d));

			}
		}



		public static double GetWidthToHeight(FrameworkElement obj)
		{
			return (double)obj.GetValue(WidthToHeightProperty);
		}

		public static void SetWidthToHeight(FrameworkElement obj, double value)
		{
			obj.SetValue(WidthToHeightProperty, value);
		}

		// Using a DependencyProperty as the backing store for Proportionate.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty WidthToHeightProperty =
			DependencyProperty.RegisterAttached("WidthToHeight", typeof(double), typeof(AProperty), new PropertyMetadata(-1.0, ProportionalChanged));

		private static void ProportionalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is FrameworkElement element)
			{
				FrameworkElement parent = (FrameworkElement)element.Parent;
				SizeChangedElement changedElement = GetSizeChangeElement(element);

				if (changedElement == null || changedElement.Parent != parent)
				{
					if (changedElement?.Parent != null)
						changedElement.Parent.SizeChanged -= changedElement.OnSizeChanged;
					SizeChangedElement sizeChanged = new SizeChangedElement(element, (FrameworkElement)element.Parent);
					if (sizeChanged.Parent != null)
						sizeChanged.Parent.SizeChanged += sizeChanged.OnSizeChanged;
					SetSizeChangeElement(element, sizeChanged);
				}
			}
			else
			{
				throw new ArgumentException("Не верный тип. Должен быть FrameworkElement");
			}
		}




		private static SizeChangedElement GetSizeChangeElement(DependencyObject obj)
		{
			return (SizeChangedElement)obj.GetValue(SizeChangeElementProperty);
		}

		private static void SetSizeChangeElement(DependencyObject obj, SizeChangedElement value)
		{
			obj.SetValue(SizeChangeElementProperty, value);
		}

		// Using a DependencyProperty as the backing store for SizeChangeElement.  This enables animation, styling, binding, etc...
		private static readonly DependencyProperty SizeChangeElementProperty =
			DependencyProperty.RegisterAttached("SizeChangeElement", typeof(SizeChangedElement), typeof(AProperty), new PropertyMetadata(null));


		private class SizeChangedElement
		{
			public FrameworkElement Element { get; }
			public FrameworkElement Parent { get; }

			public SizeChangedElement(FrameworkElement element)
			{
				Element = element;
			}

			public SizeChangedElement(FrameworkElement element, FrameworkElement parent) : this(element)
			{
				Parent = parent;
			}

			public void OnSizeChanged(object sender, SizeChangedEventArgs e)
			{

				FrameworkElement parent = (FrameworkElement)sender;
				if (parent == null) return;
				double parentWidth = parent.ActualWidth;
				double parentHeight = parent.ActualHeight;
				double widthToHeight = GetWidthToHeight(Element);
				double width;
				double height;
				if (parentWidth * widthToHeight < parentHeight)
				{
					width = parentWidth;
					height = parentWidth * widthToHeight;
				}
				else
				{
					width = parentHeight / widthToHeight;
					height = parentHeight;
				}
				Element.Width = width;
				Element.Height = height;
				//throw new NotImplementedException();
			}
		}

		//private static void OnSizeChanged(object sender, SizeChangedEventArgs e)
		//{

		//	FrameworkElement element = (FrameworkElement)sender;
		//	FrameworkElement parent = element.Parent as FrameworkElement;
		//	if (parent == null) return;
		//	double parentWidth = parent.ActualWidth;
		//	double parentHeight = parent.ActualHeight;
		//	double widthToHeight = GetWidthToHeight(element);
		//	double width;
		//	double height;
		//	if (parentWidth * widthToHeight < parentHeight)
		//	{
		//		width = parentWidth;
		//		height = parentWidth * widthToHeight;
		//	}
		//	else
		//	{
		//		width = parentHeight / widthToHeight;
		//		height = parentHeight;
		//	}
		//	element.Width = width;
		//	element.Height = height;
		//	//throw new NotImplementedException();
		//}

		//private static event
	}
}
