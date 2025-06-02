using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ML
{
    public class MCIConfig
    {
        public string TYPE { get; set; } = "MCI";
        public string IS_TRAIN { get; set; } = "True";
        public string FORMAT { get; set; } = "csv";
        public string TRAIN_DATA { get; set; } = "data/MCI_train_data.csv";
        public string TEST_DATA { get; set; } = "data/MCI_train_data.csv";
        public string THRESHOLD { get; set; } = "0.5";
        public string NTREE { get; set; } = "1000";
        public string MTRY { get; set; } = "3";
        public string KFOLD { get; set; } = "5";
    }
}
