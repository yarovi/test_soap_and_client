﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WS.Unit06.User.Application.Model
{
    public class GroupDTO
    {
		//[JsonProperty("idGroupCategory")]
		public int Id { get; set; }
        public string Name { get; set; }
    }
}
