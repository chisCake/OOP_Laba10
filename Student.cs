using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace OOP_Laba10 {
	class Student : IEnumerable {
		public string Surname { get; private set; }
		public string Name { get; private set; }
		public int Course { get; private set; }
		public Dictionary<string, int?> Marks { get; private set; }

		public Student(string surname, string name, Dictionary<string, int?> marks = null, int course = 1) {
			Surname = surname;
			Name = name;
			Marks = marks ?? new Dictionary<string, int?>();
			Course = course;
		}

		public IEnumerator GetEnumerator() {
			return Marks.GetEnumerator();
		}

		public bool SetMark(string subject, int mark) {
			if (mark < 1 || mark > 10)
				return false;

			if (!Marks.ContainsKey(subject))
				Marks.Add(subject, mark);
			else
				Marks[subject] = mark;
			return true;
		}

		public static void Add(ref List<Student> students, Student student) {
			students.Add(student);
		}

		public static bool Delete(ref List<Student> students, Student student) {
			return students.Remove(student);
		}

		public static bool Delete(ref List<Student> students, string findStr) {
			return students.Remove(Find(students, findStr));
		}

		public static bool Delete(ref ICollection<Student> students, string findStr) {
			return students.Remove(Find(students, findStr));
		}

		public static Student Find(ICollection<Student> students, string findStr) {
			string[] findParams = findStr.Split(' ');
			string surname, name = "";
			int course = 0;

			switch (findParams.Length) {
				case 1:
					surname = findParams[0];
					break;
				case 2:
					surname = findParams[0];
					if (!int.TryParse(findParams[1], out course))
						name = findParams[1];
					break;
				case 3:
					surname = findParams[0];
					name = findParams[1];
					int.TryParse(findParams[2], out course);
					break;
				default:
					return null;
			}

			foreach (var student in students) {
				if (surname == student.Surname && name == student.Name && course == student.Course ||
					surname == student.Surname && name == student.Name ||
					surname == student.Surname && course == student.Course ||
					surname == student.Surname)
					return student;
			}

			return null;
		}

		public static void Print(List<Student> students) {
			var sorted = students.OrderBy(item => item.Course).ThenBy(item => item.Surname);
			Console.WriteLine("   Фамилия         Имя         Курс      Средний балл");
			foreach (var student in sorted)
				Console.WriteLine($"{student.Surname,-13} {student.Name,-13} {student.Course,6}     {student.Marks.Values.Average(),10}      ");
		}

		public void Print() {
			Console.WriteLine(
				$"Фамилия: {Surname}" +
				$"\nИмя:     {Name}" +
				$"\nКурс:    {Course}" +
				$"\n\nОценки:"
				);
			foreach (var item in Marks)
				Console.WriteLine($"{item.Key,-15}: {GetValue(item)}");

			static string GetValue(KeyValuePair<string, int?> pair) => pair.Value.HasValue ? pair.Value.ToString() : "-";
		}

		public override string ToString() {
			return Surname + " " + Name;
		}
	}
}
