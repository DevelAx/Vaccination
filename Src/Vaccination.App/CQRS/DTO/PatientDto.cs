using System;
using System.ComponentModel;
using Vaccination.App.Mapping;
using Vaccination.Domain.Entities;

namespace Vaccination.App.CQRS.DTO
{
	public class PatientDto : IMapFrom<Patient>
	{
		public int IntId { get; set; }

		[DisplayName("Имя")]
		public string FirstName { get; set; }

		[DisplayName("Фамилия")]
		public string LastName { get; set; }

		[DisplayName("Отчество")]
		public string Patronymic { get; set; }

		[DisplayName("Дата рождения")]
		public DateTime BirthDate { get; set; }

		[DisplayName("Пол")]
		public string Sex { get; set; }

		[DisplayName("СНИЛС")]
		public string InsuranceNumber { get; set; }

		[DisplayName("ФИО")]
		public string FullName
		{
			get
			{
				return _fullName ??= string.Join(" ", LastName, FirstName, Patronymic).TrimEnd();
			}
		}

		private string _fullName;
	}
}
