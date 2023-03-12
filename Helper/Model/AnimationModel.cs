#pragma warning disable 0,
// ReSharper disable CheckNamespace MemberHidesStaticFromOuterClass

using Helper;

namespace Helper {

	public static class AnimationModel {

		#region type

		public record Rectangle {
			public required List<Floating> Position;
			public required List<Floating> Size;
		}

		public record LayerRemove {
			public required Integer Index;
		}

		public record LayerAppend {
			public required Integer  Index;
			public required String?  Name;
			public required Integer  Resource;
			public required Boolean  Sprite;
			public required Boolean  Additive;
			public required Integer  PreloadFrame;
			public required Floating TimeScale;
		}

		public record LayerChange {
			public required Integer         Index;
			public required List<Floating>  Transform;
			public required List<Floating>? Color;
			public required Integer?        SpriteFrameNumber;
			public required Rectangle?      SourceRectangle;
		}

		public record Frame {
			public required String?            Label;
			public required Boolean            Stop;
			public required List<List<String>> Command;
			public required List<LayerRemove>  Remove;
			public required List<LayerAppend>  Append;
			public required List<LayerChange>  Change;
		}

		public record Sprite {
			public required String        Name;
			public required Floating      FrameRate;
			public required List<Integer> WorkArea;
			public required List<Frame>   Frame;
		}

		public record Image {
			public required String         Name;
			public required List<Integer>  Size;
			public required List<Floating> Transform;
		}

		public record Animation {
			public required Integer        FrameRate;
			public required List<Floating> Position;
			public required List<Floating> Size;
			public required List<Image>    Image;
			public required List<Sprite>   Sprite;
			public required Sprite?        MainSprite;
		}

		#endregion

	}

}