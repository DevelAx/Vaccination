using AutoMapper;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Vaccination.App.AuxiliaryTypes;
using Vaccination.App.Mapping;
using Vaccination.Domain.Entities;
using Vaccination.Infastructure.Utils;

namespace Vaccination.App.CQRS.Patients.Commands.Base
{
	public abstract class BasePatientEditVM : BaseMapFrom<Patient>
	{
		[DisplayName("Имя")]
		public string FirstName { get; set; }

		[DisplayName("Фамилия")]
		public string LastName { get; set; }

		[DisplayName("Отчество")]
		public string Patronymic { get; set; }

		[DisplayName("Дата рождения")]
		public DateTime? BirthDate { get; set; }

		[DisplayName("Пол")]
		public Sexes? Sex { get; set; }

		[DisplayName("СНИЛС")]
		public string InsuranceNumber { get; set; }

		public Vaccine[] AllVaccines { get; set; }

		#region Mapping

		protected static void Mapping<TDestination>(Profile profile)
			where TDestination : BasePatientEditVM
		{
			profile.CreateMap<Patient, TDestination>()
				.ForMember(vm => vm.Sex, opt => opt.MapFrom(p => SexAsEnum(p.Sex)));

			profile.CreateMap<TDestination, Patient>()
				.ForMember(p => p.Sex, opt => opt.MapFrom(vm => SexAsText(vm.Sex.Value)));
		}

		private static Sexes SexAsEnum(string sex)
		{
			var enumType = typeof(Sexes);

			foreach (Sexes sexEnum in (Sexes[])Enum.GetValues(typeof(Sexes)))
			{
				string sexName = FindAttribute<DisplayAttribute>.InEnum(sexEnum).Name;

				if (sexName == sex)
					return sexEnum;
			}

			throw new ArgumentOutOfRangeException($"The '{sex}' value is not found.");
		}

		private static string SexAsText(Sexes sex)
		{
			string sexName = FindAttribute<DisplayAttribute>.InEnum(sex).Name;
			return sexName;
		}

		#endregion
	}
}
