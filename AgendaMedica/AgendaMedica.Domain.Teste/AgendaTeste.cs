﻿using AgendaMedica.Domain.Entities;
using AgendaMedica.Domain.Interfaces.Domain;
using AgendaMedica.Domain.Interfaces.Repositories;
using AgendaMedica.Domain.Services;
using AgendaMedica.Domain.Validations;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgendaMedica.Domain.Teste
{
    [TestFixture]
    public class AgendaTeste
    {
        private Agenda agenda;
        private IAgendaService _agendaService;

        [SetUp]
        public void Setup()
        {
            agenda = new Agenda
            {
                AgendaId = 0,
                DataHoraInicio = new DateTime(2019, 05, 17, 14, 30, 0),
                DataHoraFim = new DateTime(2019, 10, 17, 14, 30, 0)
            };
        }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var mock = new Mock<IAgendaRepository>();
            mock.Setup(x => x.Add(It.IsAny<Agenda>()));
            _agendaService = new AgendaService(mock.Object);
        }

        [Test]
        [Category("Cadastrar nova Agenda")]
        public void AgendaDataInicioMenorQueDataFim()
        {
            agenda.DataHoraInicio = new DateTime(2019, 05, 17, 14, 30, 0);
            agenda.DataHoraFim = new DateTime(2019, 05, 17, 12, 30, 0);

            _agendaService.Add(agenda);

            Assert.IsNotEmpty(agenda.ValidationResult.Errors.Where(e => e.ErrorMessage == AgendaValidator.ErrorsMessages["DATA_FIM_MAIOR_QUE_DATA_INICIO"]));
        }

        [Test]
        [Category("Adicionar horário(s) à Agenda")]
        public void NaoDeveHaverConflitosDeHorarios()
        {
            agenda.Horarios = new List<Horario>
            {
                new Horario
                {
                    DiaSemana = DiaSemana.Segunda,
                    HoraInicio = new TimeSpan(7, 30, 0),
                    HoraFim = new TimeSpan(8, 30, 0)
                },
                new Horario
                {
                    DiaSemana = DiaSemana.Segunda,
                    HoraInicio = new TimeSpan(8, 0, 0),
                    HoraFim = new TimeSpan(9, 0, 0)
                }
            };

            _agendaService.Add(agenda);

            Assert.IsNotEmpty(agenda.ValidationResult.Errors.Where(e => e.ErrorMessage == AgendaValidator.ErrorsMessages["HORARIOS_CONFLITAM"]));
        }
    }
}