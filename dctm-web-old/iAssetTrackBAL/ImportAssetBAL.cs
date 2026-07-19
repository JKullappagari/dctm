using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace iAssetTrackBAL
{
    [Serializable]
    public class ImportAssetBAL
    {
        private string _hostName;
        private string _serialNumber;
        private string _model;
        private string _manufacture;
        private string _rackID;
        private string _rowID;
        private string _room;
        private string _site;
        private int _noOfRUs;
        private int _startPos;
        private string _equipmentType;
        private string _rackStand;

        # region Properties

        [XmlElement( IsNullable = true )]
        public string HostName
        {
            get
            { return _hostName; }
            set
            { _hostName = value; }
        }

        [XmlElement(IsNullable = true)]
        public string SerialNumber
        {
            get
            { return _serialNumber; }
            set
            { _serialNumber = value; }
        }

        [XmlElement(IsNullable = true)]
        public string Model
        {
            get
            { return _model; }
            set
            { _model = value; }
        }

        [XmlElement(IsNullable = true)]
        public string Manufacture
        {
            get
            { return _manufacture; }
            set
            { _manufacture = value; }
        }

        [XmlElement(IsNullable = true)]
        public string RackID
        {
            get
            { return _rackID; }
            set
            { _rackID = value; }
        }

        [XmlElement(IsNullable = true)]
        public string RowID
        {
            get
            { return _rowID; }
            set
            { _rowID = value; }
        }

        [XmlElement(IsNullable = true)]
        public string Room
        {
            get
            { return _room; }
            set
            { _room = value; }
        }

        [XmlElement(IsNullable = true)]
        public string Site
        {
            get
            { return _site; }
            set
            { _site = value; }
        }

        [XmlElement(IsNullable = true)]
        public int NoOfRUs
        {
            get
            { return _noOfRUs; }
            set
            { _noOfRUs = value; }
        }

        [XmlElement(IsNullable = true)]
        public int StartPosition
        {
            get
            { return _startPos; }
            set
            { _startPos = value; }
        }

        [XmlElement(IsNullable = true)]
        public string Equipmenttype
        {
            get
            { return _equipmentType; }
            set
            { _equipmentType = value; }
        }

        [XmlElement(IsNullable = true)]
        public string RackStand
        {
            get
            { return _rackStand; }
            set
            { _rackStand = value; }
        }

        # endregion

        #region Constructors

        public ImportAssetBAL(string _HostName,string _SerailNo,string _Model,string _Manufacture,
                string _RackID,string _RowID,string _Room,string _Site,int _NoOfRUs,
                int _StartPos,string _EquipmentType,string _RackStand)
        {
            HostName = _hostName;
            SerialNumber = _SerailNo;
            Model = _Model;
            Manufacture = _Manufacture;
            RackID = _RackID;
            RowID = _RowID;
            Room = _Room;
            Site = _Site;
            NoOfRUs = _NoOfRUs;
            StartPosition = _StartPos;
            Equipmenttype = _EquipmentType;
            RackStand = _RackStand;


        }

        # endregion


    }
}
