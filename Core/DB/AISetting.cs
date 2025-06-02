using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DB
{
	public class AISetting : TableBase
	{
		public Environment environment { get; set; }
		public Type type { get; set; }
		//public string trainData { get; set; }// = "MCI_train_data.csv";MLEduFiles/id/data/test.csv
		//public string testData { get; set; }// = "MCI_train_data.csv";MLEduFiles/id/data/train.csv
		public decimal threshold { get; set; }// = "0.5";
		public int ntree { get; set; }// = "1000";
		public int mtry { get; set; }// = "3";
		public int kfold { get; set; }// = "5";

		public bool active { get; set; }
		public Status status { get; set; }
		public bool testCSV { get; set; } = false;
		public bool trainCSV { get; set; } = false;

		public enum Environment
		{
			RD = 0,
			LAB = 1
		}

		public enum Type
		{
			AD = 0,//MCI
			PD = 1
		}

		public enum Status
		{
			training = 0,
			trainingCompleted = 1,
			trainingError = 2,
			waitForTraining = 3
		}
	}
}
