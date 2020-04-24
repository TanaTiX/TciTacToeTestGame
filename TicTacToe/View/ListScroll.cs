using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace View
{

	public class ListScroll : UserControl
	{
		/// <summary>Источник коллекции</summary>
		public IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ItemsSourceProperty =
			DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(ListScroll),
				new PropertyMetadata(null, (d, e) => ((ListScroll)d).ItemsSourceChanged(e)));

		private void ItemsSourceChanged(DependencyPropertyChangedEventArgs e)
		{
			if (SelectedItem == null)
				SelectItemFromIndex((IEnumerable)e.NewValue);
			else
				SelectIndexFromItem((IEnumerable)e.NewValue);
		}

		/// <summary>Флаг индикатор работы метода SelectItemFromIndex</summary>
		protected bool isSelectedIndexChanged;
		/// <summary>Установка выделенного элемента по выделенному индексу и заданной последовательности</summary>
		/// <param name="items">Заданная последовательность</param>
		protected void SelectItemFromIndex(IEnumerable items)
		{
			if (items == null)
				return;

			// Для избежания зацикливания
			if (isSelectedIndexChanged)
				return;
			isSelectedIndexChanged = true;

			int i = 0;
			object sItem = null;
			int selectedIndex = SelectedIndex;
			foreach (object item in items)
			{
				if (i > selectedIndex)
					break;
				if (i == selectedIndex)
				{
					sItem = item;
					break;
				}
				i++;
			}
			SelectedItem = sItem;

			isSelectedIndexChanged = false;
		}

		/// <summary>Установка индекса по выделенному элементу и заданной последовательности</summary>
		/// <param name="items">Заданная последовательность</param>
		protected void SelectIndexFromItem(IEnumerable items)
		{
			if (items == null)
				return;

			int i = 0;
			int ind = -1;
			object selectedItem = SelectedItem;
			foreach (object item in items)
			{
				if (item == selectedItem)
				{
					ind = i;
					break;
				}
				i++;
			}
			SelectedIndex = ind;
		}


		/// <summary>Индекс выделенного элемента</summary>
		public int SelectedIndex
		{
			get { return (int)GetValue(SelectedIndexProperty); }
			set { SetValue(SelectedIndexProperty, value); }
		}

		// Using a DependencyProperty as the backing store for SelectedIndex.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SelectedIndexProperty =
			DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(ListScroll),
				new FrameworkPropertyMetadata(-1, (d, e) => ((ListScroll)d).SelectedIndexChanged(e)) { DefaultUpdateSourceTrigger=System.Windows.Data.UpdateSourceTrigger.PropertyChanged, BindsTwoWayByDefault=true});

		private void SelectedIndexChanged(DependencyPropertyChangedEventArgs e)
		{

			SelectItemFromIndex(ItemsSource);

		}


		/// <summary>Выделенный элемент</summary>
		public object SelectedItem
		{
			get { return (object)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		// Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SelectedItemProperty =
			DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(ListScroll),
				new FrameworkPropertyMetadata(null, (d, e) => ((ListScroll)d).SelectedItemChanged(e)){ DefaultUpdateSourceTrigger=System.Windows.Data.UpdateSourceTrigger.PropertyChanged, BindsTwoWayByDefault=true});

		private void SelectedItemChanged(DependencyPropertyChangedEventArgs e)
		{

			if (!isSelectedIndexChanged)

				SelectItemFromIndex(ItemsSource);

		}

		/// <summary>Пропускаемые индексы</summary>
		public IList<int> SkippingIndexes
		{
			get { return (IList<int>)GetValue(SkippingIndexesProperty); }
			set { SetValue(SkippingIndexesProperty, value); }
		}

		// Using a DependencyProperty as the backing store for SkippingIndexes.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SkippingIndexesProperty =
			DependencyProperty.Register(nameof(SkippingIndexes), typeof(IList<int>), typeof(ListScroll), new PropertyMetadata(null));

		private ICommand _nextIndexCommand;
		public ICommand NextIndexCommand => _nextIndexCommand
			?? (_nextIndexCommand = new RelayCommand(NextIndexMethod, NextIndexCanMethod));

		/// <summary>Метод проверки команды</summary>
		/// <param name="parameter">Параметр команды</param>
		/// <returns><see langword="true"/> если команда исполнима</returns>
		/// <remarks>В методе проверяется наличие в коллекции хотя бы двух элементов
		/// и приводимость параметра к целому числу</remarks>
		protected bool NextIndexCanMethod(object parameter)
		{
			if (!(ItemsSource?.OfType<object>().Skip(1).Any() == true))
				return false;

			return IntTryParse(parameter, out _);
		}

		/// <summary>Метод получающий int из object</summary>
		/// <param name="value">Объект со значением</param>
		/// <param name="number">Возвращаемое целое число</param>
		/// <returns><see langword="true"/> если удалось получить целое число</returns>
		public static bool IntTryParse(object value, out int number)
		{
			if (value == null)
			{
				number = default;
				return false;
			}
			if (value is int valI)
			{
				number = valI;
				return true;
			}
			if (value is string valS)
				return int.TryParse(valS, out number);
			return int.TryParse(value.ToString(), out number);
		}

		/// <summary>Метод исполнения команды</summary>
		/// <param name="parameter">Параметр команды</param>
		protected void NextIndexMethod(object parameter)
		{
			if (!(ItemsSource?.Cast<object>().Skip(1).Any() == true && IntTryParse(parameter, out int addIndex)))
				return;

			// Количество элементов в коллекции
			int countItems = ItemsSource.Cast<object>().Count();

			// Получение отсортированного списка положительных индексов
			var skipping = SkippingIndexes? // Получаем все пропускаемые индексы
				.Distinct() // Удаляем все дубликаты
				.OrderBy(i => i) // Сортируем по возрастанию
				.SkipWhile(i => i < 0) // Пропускаем все меньшие нуля
				.TakeWhile(i => i < countItems) // Забираем все меньше количества элементов
				.ToList() // Преобразуем в List
				?? new List<int>(); // Защита на случай нулевой коллекции

			int countActiv = countItems - skipping.Count; // Количество элементов которые можно выбирать

			// Если выбирать нечего - выход
			if (countActiv <= 0)
			{
				SelectedIndex = -1;
				return;
			}

			// Получение смещения с учётом пропускаемых индексов и зацикливания
			addIndex = ((addIndex % countActiv) + countActiv) % countActiv;

			// Если смещения нет, то находим с текущего индекса первый активный
			if (addIndex == 0)
			{
				int index = SelectedIndex;
				while (skipping.BinarySearch(index) < 0)
				{
					index = (index + 1) % countItems;
				}
				SelectedIndex = index;
				return;
			}

			// Делаем заданное количество шагов с пропуском неактивных
			{
				int index = SelectedIndex;
				for (int i = 0; i < addIndex; i++)
				{
					while (skipping.BinarySearch(index) >= 0)
					{
						index = (index + 1) % countItems;
					}
					index = (index + 1) % countItems;
				}
				SelectedIndex = index;
				return;
			}
		}




		public object Title
		{
			get { return (object)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		// Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register(nameof(Title), typeof(object), typeof(ListScroll), new FrameworkPropertyMetadata(null) { DefaultUpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged, BindsTwoWayByDefault = true });


	}

}
