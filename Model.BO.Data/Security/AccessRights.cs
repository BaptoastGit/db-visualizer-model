using Model.BO.Data.Model;
using System.Text.Json.Nodes;

namespace Model.BO.Data.Security
{
    public class AccessRights
    {
        public static Dictionary< string, List<string> > getAccessRightGroups()
        {
            Dictionary< string, List<string> > accessGroups = [];

            string jsonString = File.ReadAllText("wwwroot/data/CRUDrights.json");
            JsonNode node = JsonNode.Parse(jsonString);

            if (node != null && node["groups"] is JsonObject adGroupsObject)
            {
                foreach (var groupProperty in adGroupsObject)
                {
                    foreach (var useCase in groupProperty.Value.AsObject())
                    {

                        if (!accessGroups.ContainsKey(string.Concat("Admin", useCase.Key)))
                            accessGroups.Add(string.Concat("Admin", useCase.Key), []);

                        if (!accessGroups.ContainsKey(string.Concat("Editor", useCase.Key)))
                            accessGroups.Add(string.Concat("Editor", useCase.Key), []);

                        if (!accessGroups.ContainsKey(string.Concat("Reader", useCase.Key)))
                            accessGroups.Add(string.Concat("Reader", useCase.Key), []);


                        if (useCase.Value.ToString() == "admin")
                            accessGroups[string.Concat("Admin", useCase.Key)].Add(groupProperty.Key);

                        if (useCase.Value.ToString() == "editor" || useCase.Value.ToString() == "admin")
                            accessGroups[string.Concat("Editor", useCase.Key)].Add(groupProperty.Key);

                        if (useCase.Value.ToString() == "reader" || useCase.Value.ToString() == "editor" || useCase.Value.ToString() == "admin")
                            accessGroups[string.Concat("Reader", useCase.Key)].Add(groupProperty.Key);

                    }

                }


            }
            foreach (var group in accessGroups)
            {
                Console.WriteLine(group.Key +":  "+ string.Join(", ", group.Value));
            }
            return accessGroups;
        }

    }
}
     