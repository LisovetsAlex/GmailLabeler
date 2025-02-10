using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskEmailCategorize
{
    public class ApiSettings
    {
        public string ImapAddress { get; set; }
        public string ImapPass { get; set; }
        public string OpenAiApiKey { get; set; }
        public string OpenAiOrgKey { get; set; }
        public string OpenAiProjKey { get; set; }
        public List<CategoryModel> Categories { get; set; }
    }

}
