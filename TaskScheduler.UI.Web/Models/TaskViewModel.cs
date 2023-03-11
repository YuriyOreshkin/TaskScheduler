using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TaskScheduler.Domain;

namespace TaskScheduler.UI.Web.Models
{
    public class TaskViewModel
    {
       
        public long id { get; set; }
        [Required(ErrorMessage = "Требуется поле {0} !")]
        [DisplayName("Наименование")]
        public string name { get; set; }
        [Required(ErrorMessage = "Требуется поле {0} !")]
        [DisplayName("Команда")]
        [Url(ErrorMessage = "Не верный формат поля {0} !")]
        public string job { get; set; }
        
        [DisplayName("Следующий запуск")]
        public DateTime? nextfiretime { get; set; }

        [DisplayName("Состояние")]
        public bool isStarted { get; set; }

        [Required(ErrorMessage = "Установите интервал запуска!")]
        [Range(1, 1440, ErrorMessage = "Интервал должен находится в диапазоне от {1} до {2}.")]
        public int intervalinminutes { get; set; }

        [Required(ErrorMessage = "Установите время ежедневного запуска работы!")]
        public DateTime startingdailyat { get; set; }
        [Required(ErrorMessage = "Установите время ежедневного окончания работы!")]
        public DateTime endingdailyat { get; set; }

        [Required(ErrorMessage = "Выберите дни недели запуска работы!")]
        public string[] ondaysoftheweek { get; set; }

        

        public TASK ToEntity(TASK entity)
        {
            entity.ID = id;
            entity.NAME = name;
            entity.JOB = job;
            entity.SCHEDULER = ToEntity(entity.SCHEDULER != null ? entity.SCHEDULER : new SCHEDULER() );

            return entity;
        }

        private SCHEDULER ToEntity(SCHEDULER entity)
        {

            entity.INTERVALINMINUTES = intervalinminutes;
            entity.STARTINGDAILYAT = startingdailyat.TimeOfDay.ToString();
            entity.ENDINGDAILYAT = endingdailyat.TimeOfDay.ToString();
            entity.ONDAYSOFTHEWEEK = String.Join("|", ondaysoftheweek);

            return entity;
        }
    }
}