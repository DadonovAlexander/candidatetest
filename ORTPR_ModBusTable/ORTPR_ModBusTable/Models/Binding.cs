using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ORTPR_ModBusTable.Models
{
    public class Binding
    {
        const string _Introdused = "Introduced";    //TODO: вероятно, есть более правильный путь создания константного аттрибута  
        /// <summary>
        /// Константный атрибут
        /// </summary>
        [XmlAttribute("Binding")]
        public string Attribute { get { return _Introdused; } set { } }
        /// <summary>
        /// Полное наименование тега
        /// </summary>
        [XmlElement("node-path")]
        public string Tag { get; set; }
        /// <summary>
        /// Тип данных
        /// </summary>
        [XmlIgnore]
        public string Type { get; set; }
        /// <summary>
        /// MobBus-адресс
        /// </summary>
        [XmlElement("address")]
        public int Address { get; set; }

        public Binding() { }

        public Binding(string tag, string type, int address)
        {
            Tag = tag;
            Type = type;
            Address = address;
        }
    }
    [XmlRoot("root")]
    public class Bindings
    {
        /// <summary>
        /// Список "привязок" структуры устройств к карте ModBus 
        /// </summary>
        [XmlElement("item")]
        public List<Binding> Items { get; set; }

        public Bindings()
        {
            Items = new List<Binding>();
        }
        /// <summary>
        /// Добавление "привязки" в список
        /// </summary>
        /// <param name="item"></param>
        public void Add(Binding item)
        {
            Items.Add(item);
        }
    }
}
