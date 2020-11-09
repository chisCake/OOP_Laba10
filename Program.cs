using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace OOP_Laba10 {
	class Program {
		static readonly List<Action> Tasks = new List<Action> {
			new Action(Task1),
			new Action(Task2)
		};

		static void Main() {
			while (true) {
				Console.Write(
					"1 - класс с интерфейсом в коллекции и наблюдаемая коллекция" +
					"\n2 - универсальная коллекция с данными встроенного типа" +
					"\n0 - выход" +
					"\nВыберите действие: "
					);
				if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 0 || choice > Tasks.Count) {
					Console.WriteLine("Нет такого действия");
					Console.ReadKey();
					Console.Clear();
					continue;
				}
				if (choice == 0) {
					Console.WriteLine("Выход...");
					return;
				}

				Tasks[choice - 1]();

				Console.ReadKey();
				Console.Clear();
			}
		}

		static void Task1() {
			var students = new List<Student>(Generator.Generate(50));
			var observable = new ObservableCollection<Student>(students);
			observable.CollectionChanged += Notify;

			while (true) {
				Console.Clear();
				Console.Write(
					"1 - добавить студента" +
					"\n2 - удалить студента" +
					"\n3 - поиск студента" +
					"\n4 - вывод списка студентов" +
					"\n0 - назад" +
					"\nВыберите действие: "
					);
				if (!int.TryParse(Console.ReadLine(), out int choice)) {
					Console.WriteLine("Нет такого действия");
					Console.ReadKey();
					continue;
				}

				switch (choice) {
					case 1:
						Console.Write("Фамилия: ");
						string surname = Console.ReadLine();
						Console.Write("Имя: ");
						string name = Console.ReadLine();
						students.Add(new Student(surname, name));
						observable.Add(new Student(surname, name));
						Console.WriteLine("Студент добавлен");
						break;
					case 2: {
						Console.Write("Введите данные для поиска удаляемого студента: ");
						string findThis = Console.ReadLine();
						observable.Remove(Student.Find(observable, findThis));
						if (Student.Delete(ref students, findThis)) {
							Console.WriteLine("Студент удалён");
						}
						else
							Console.WriteLine("Студент не найден");
					}
					break;
					case 3: {
						Console.Write("Введите данные для поиска студента: ");
						string findThis = Console.ReadLine();
						var student = Student.Find(students, findThis);
						if (student != null)
							student.Print();
						else
							Console.WriteLine("Студент не найден");
					}
					break;
					case 4:
						Student.Print(students);
						break;
					case 0:
						return;
					default:
						Console.WriteLine("Действие не распознанно");
						continue;
				}

				Console.ReadKey();
			}
		}

		static void Task2() {
			var queue = new Queue<int>();
			var rndm = new Random();

			for (int i = 0; i < 10; i++)
				queue.Enqueue(rndm.Next(0, 100));

			Console.WriteLine("Полученная очередь");
			Print();
			Console.ReadKey();

			int amt;
			do {
				Console.Write($"\nВведите кол-во элементов для удаления (0 <= n < {queue.Count}): ");
			} while (!int.TryParse(Console.ReadLine(), out amt) || amt < 0 || amt > queue.Count);

			for (int i = 0; i < amt; i++)
				queue.Dequeue();

			Console.WriteLine($"Полученная очередь после удаления {amt} элементов");
			Print();
			Console.ReadKey();

			Console.WriteLine("\nВводите числа для добавления в очередь; не число - прекращение ввода");
			while (true) {
				Console.Write("Введите число: ");
				if (!int.TryParse(Console.ReadLine(), out int num))
					break;
				queue.Enqueue(num);
			}

			Console.WriteLine("\nПолученная очередь после добавления элементов");
			Print();
			Console.ReadKey();

			var hashSet = new HashSet<int>(queue);
			Console.WriteLine("\nПолученное множество, основанное на хеш-таблице");
			foreach (var item in hashSet) {
				Console.Write(item + " ");
			}
			Console.ReadKey();

			Console.WriteLine("\n\nВводите числа для поиска во множестве; не число - прекращение поиска");
			while (true) {
				Console.Write("\nВведите число: ");
				if (!int.TryParse(Console.ReadLine(), out int num))
					break;
				string res = hashSet.Contains(num) ? $"Число {num} есть" : $"Числа {num} нет";
				Console.WriteLine($"{res} во множестве");
			}

			void Print() {
				foreach (var item in queue) {
					Console.Write(item + " ");
				}
				Console.WriteLine();
			}
		}

		static void Notify(object sender, NotifyCollectionChangedEventArgs args) {
			Console.Write("Event: ");
			Console.WriteLine(
				args.Action switch
				{
					NotifyCollectionChangedAction.Add => $"Студент {args.NewItems[0] as Student} добавлен",
					NotifyCollectionChangedAction.Remove => $"Студент {args.OldItems[0] as Student} удалён",
					_ => "Неизвестное действие"
				}
				);
		}
	}
}
