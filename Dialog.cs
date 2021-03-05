using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
/*
 * Важная информация. Я долго мучался и пришел к выводу что проще частично редактировать json файл вручную. 
 * Там встречаются разыне переменные с одинаковым именем. К примеру "Value" - бывает трех типов.
 * "Text" - встречается как просто стринг, так и массив. А "variables" - вообще коллекция Dictionary где опять же "Value" разных типов. 
 * Все это можно было бы исправить, но все это надо тянуть из jsona именно так как его закомпилировала прога  Dialigue Designer из за этого
 * любые танцы с бубном могут оказаться печальны. В конечном итоге я просто привожу все к одному типу переменных - string. А строчку "variables"
 * Вообще удаляю полностью. Так же хочу обратить внимание, что весь текст в файле .json заключен в "[ ]" Скобки, что говорит десериализации, что это тоже массив.
 * Их надо тоже удалить. Еще мне пока не удалось выяснить, что за переменная - "toggle", но пока у нее стоит тип string. Если будут проблеммы стоит обратить на нее внимание.
*/
namespace DialogJson
{
    // Dialigue Designer так сериализует классы, что их сложно потом распутать. Изначально он пихает все в массив. Для этого этот класс и есть.
    public class BigClass
    {
        public List<Load_Stage> nodes { get; set; }
    }
    // В Этот класс будут загружаться все классы из программы Dialigue Designer
  public  class Load_Stage
    {
        public string file { get; set; }
        public string next { get; set; }
        public string node_name { get; set; }
        public string node_type { get; set; }
        public string object_path { get; set; }
        public string title { get; set; }
        public string ex_text { get; set; } // для Exeption так как там переменная тоже называется exeption
        public string operation_type { get; set; }
        public string value { get; set; }
        public string toggle { get; set; }// неизвестно какой тут тип Стринг или Инт
        public bool is_box { get; set; }
        public bool slide_camera { get; set; }
        public int speaker_type { get; set; }
        public int possibilites { get; set; }
        public int time { get; set; }
        public int chance_1 { get; set; }
        public int chance_2 { get; set; }
        public int rect_size_x { get; set; }
        public int rect_size_y { get; set; }
        public Text text { get; set; }
        public List<Choice> choices { get; set; }
        public Dictionary<string, string> branches { get; set; }
        //[JsonExtensionData]
        //public Dictionary<string, object> ExtensionData { get; set; }
    }
    //В это классе храниться текст с разными языками. Это текст в кнопках и содержании сцены. 
    public class Text
    {
        public string ENG { get; set; }
        public string RU { get; set; }
    }
    public class Choice
    {
        public string condition { get; set; }
        public bool is_condition { get; set; }
        public string next { get; set; }
        public Text text { get; set; }
    }
    //тут просто храняться функции с методом десериализации. Все что нужно это использовать функцию decoding где в скобках
    //нужно указать адрес к файлу. Имейте в виду, что класс статичный и его экземпляры создавать не нужно, просто используейте его.
   public static class Dekodering
    {
        public static Dictionary<string, Load_Stage> Stage = new Dictionary<string, Load_Stage>();
       
        public static void decoding(string file)
        {
            using FileStream openStream = File.OpenRead(file);
            BigClass Test = JsonSerializer.Deserialize<BigClass>(File.ReadAllText(file)); // так уж сериалзует Dialogue, что приходиться сперва в класс все гнать. 

            foreach (var item in Test.nodes)//а здесь мы список привращаем в коллекцию, где ключи это именна комнат, которые представленны в виде кода. По этим ключам совершается навигация. 
            {
                Stage.Add(item.node_name, item);
            }
        }
    }
}
