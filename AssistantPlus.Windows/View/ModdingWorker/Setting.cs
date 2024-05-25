#pragma warning disable 0,
// ReSharper disable

using AssistantPlus;
using AssistantPlus.Utility;
using Newtonsoft.Json;

namespace AssistantPlus.View.ModdingWorker {

	[JsonObject(ItemRequired = Required.AllowNull)]
	public record Setting {
		public String       Kernel          = default!;
		public String       Script          = default!;
		public List<String> Argument        = default!;
		public Boolean      AutomaticScroll = default!;
		public Boolean      ImmediateLaunch = default!;
		public String       MessageFont     = default!;
	}

}
