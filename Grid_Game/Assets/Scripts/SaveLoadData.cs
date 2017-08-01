using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public sealed class SaveLoadData : MonoBehaviour
{
    private void Awake()
    {
        
    }

    void SaveCellTypes(CellTypes toSave)
    {
        string filePth = Application.persistentDataPath + "/" + toSave.gameObject.name + ".mdcells";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(filePth, FileMode.Open);
        CellTypesIO data = new CellTypesIO(toSave);
        bf.Serialize(file, data);
        file.Close();
    }

    CellTypes LoadCellTypes(string objName)
    {
        string filePth = Application.persistentDataPath + "/" + objName + ".mdcells";
        if(File.Exists(filePth))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePth, FileMode.Open);
            CellTypesIO data = (CellTypesIO)bf.Deserialize(file);
            file.Close();
            return data.info;
        }
        return null;
    }
}

[System.Serializable]
class CellTypesIO
{
    public CellTypes info;

    public CellTypesIO (CellTypes fromMonoD)
    {
        info = new CellTypes(fromMonoD);
    }
}
