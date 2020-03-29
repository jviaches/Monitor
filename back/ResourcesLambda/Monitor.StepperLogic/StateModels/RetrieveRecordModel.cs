using Monitor.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monitor.StepperLogic.StateModels
{
    public class RetrieveRecordModel
    {
        /// <summary>
        /// Periodicity in minutes
        /// </summary>
        public int Periodicity { get; set; }
    }
}
