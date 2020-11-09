using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OOP_Laba10 {
	class Generator {
		private static Random rndm = new Random();

		// Сырые данные
		private static List<string> surnames = new List<string>();
		private static List<string> names = new List<string>();

		// Статус загруженности сырых данных
		public static bool Loaded { get; private set; }

		static Generator() {
			Loaded = false;
		}

		#region ReadingFiles
		// Загрузка сырых данных из файлов
		private static void LoadSurnames() {
			using var sr = new StreamReader("surnames.txt");
			string surname;
			while ((surname = sr.ReadLine()) != null)
				surnames.Add(surname);
		}

		private static void LoadNames() {
			using var sr = new StreamReader("names.txt");
			string name;
			while ((name = sr.ReadLine()) != null)
				names.Add(name);
		}

		private static bool LoadBlanks() {
			try {
				LoadSurnames();
				LoadNames();
				return true;
			}
			catch {
				Console.WriteLine("Не удалось считать файлы");
				return false;
			}
		}
		#endregion

		// Генерация абитуриентов
		public static Student[] Generate(int amt) {
			if (!Loaded)
				if (!LoadBlanks()) {
					Console.WriteLine("Студенты сгенерированы не были");
					return null;
				}

			Student[] students = new Student[amt];

			for (int i = 0; i < amt; i++) {
				// Генерация ФИО
				string surname = surnames[rndm.Next(surnames.Count)];
				string name = names[rndm.Next(names.Count)];

				int course = rndm.Next(1, 5);

				// Генерация оценок
				int[] minVals = { 1, 4, 7 }, maxVals = { 6, 8, 10 };
				// Определение уровня интеллекта
				int iqBorder = rndm.Next(1, 6),
					iqLvl = iqBorder == 1 ? 0 : iqBorder == 5 ? 2 : 1;
				// Создание словаря
				var marks = new Dictionary<string, int?> {
					{"Предмет1", rndm.Next(minVals[iqLvl], maxVals[iqLvl])},
					{"Предмет2", rndm.Next(minVals[iqLvl], maxVals[iqLvl])},
					{"Предмет3", rndm.Next(minVals[iqLvl], maxVals[iqLvl])},
					{"Предмет4", rndm.Next(minVals[iqLvl], maxVals[iqLvl])},
					{"Предмет5", rndm.Next(minVals[iqLvl], maxVals[iqLvl])}
				};

				students[i] = new Student(surname, name, marks, course);
			}

			return students;
		}
	}
}
