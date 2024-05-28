#pragma warning disable 0,
// ReSharper disable CheckNamespace

using AssistantPlus;
using AssistantPlus.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AssistantPlus {

	public static class GameReflectionHelper {

		#region utility

		public class CompositeTypeJsonConverter : JsonConverter {

			public override Boolean CanConvert (
				Type objectType
			) {
				return objectType == typeof(GameReflectionModel.CompositeType);
			}

			public override void WriteJson (
				JsonWriter     writer,
				Object?        value,
				JsonSerializer serializer
			) {
				throw new NotImplementedException();
			}

			public override Object? ReadJson (
				JsonReader     reader,
				Type           objectType,
				Object?        existingValue,
				JsonSerializer serializer
			) {
				var token = serializer.Deserialize<String>(reader);
				if (token is null) {
					return null;
				}
				return GameReflectionHelper.ParseTypeExpression(token);
			}

		}

		// ----------------

		public static GameReflectionModel.CompositeType ParseTypeExpression (
			List<String> ripe
		) {
			var raw = new GameReflectionModel.CompositeType() {
				Primitive = default!,
				Element = null,
				Class = null,
			};
			switch (ripe[0]) {
				case nameof(GameReflectionModel.PrimitiveType.Boolean): {
					GF.AssertTest(ripe.Count == 1);
					raw.Primitive = GameReflectionModel.PrimitiveType.Boolean;
					break;
				}
				case nameof(GameReflectionModel.PrimitiveType.Integer): {
					GF.AssertTest(ripe.Count == 1);
					raw.Primitive = GameReflectionModel.PrimitiveType.Integer;
					break;
				}
				case nameof(GameReflectionModel.PrimitiveType.Floater): {
					GF.AssertTest(ripe.Count == 1);
					raw.Primitive = GameReflectionModel.PrimitiveType.Floater;
					break;
				}
				case nameof(GameReflectionModel.PrimitiveType.String): {
					GF.AssertTest(ripe.Count == 1);
					raw.Primitive = GameReflectionModel.PrimitiveType.String;
					break;
				}
				case nameof(GameReflectionModel.PrimitiveType.Enumeration): {
					GF.AssertTest(ripe.Count == 2);
					raw.Primitive = GameReflectionModel.PrimitiveType.Enumeration;
					raw.Class = ripe[1];
					break;
				}
				case nameof(GameReflectionModel.PrimitiveType.Reference): {
					GF.AssertTest(ripe.Count == 2);
					raw.Primitive = GameReflectionModel.PrimitiveType.Reference;
					raw.Class = ripe[1];
					break;
				}
				case nameof(GameReflectionModel.PrimitiveType.List): {
					GF.AssertTest(ripe.Count >= 2);
					raw.Primitive = GameReflectionModel.PrimitiveType.List;
					raw.Element = GameReflectionHelper.ParseTypeExpression(ripe[1..]);
					break;
				}
				case nameof(GameReflectionModel.PrimitiveType.Map): {
					GF.AssertTest(ripe.Count >= 2);
					raw.Primitive = GameReflectionModel.PrimitiveType.Map;
					raw.Element = GameReflectionHelper.ParseTypeExpression(ripe[1..]);
					break;
				}
				case nameof(GameReflectionModel.PrimitiveType.Object): {
					GF.AssertTest(ripe.Count == 2);
					raw.Primitive = GameReflectionModel.PrimitiveType.Object;
					raw.Class = ripe[1];
					break;
				}
				default: throw new ();
			}
			return raw;
		}

		public static GameReflectionModel.CompositeType ParseTypeExpression (
			String ripe
		) {
			return GameReflectionHelper.ParseTypeExpression(ripe.Split(".").ToList());
		}

		public static String MakeTypeName (
			GameReflectionModel.CompositeType ripe
		) {
			return ripe.Primitive switch {
				GameReflectionModel.PrimitiveType.Boolean     => $"{ripe.Primitive}",
				GameReflectionModel.PrimitiveType.Integer     => $"{ripe.Primitive}",
				GameReflectionModel.PrimitiveType.Floater     => $"{ripe.Primitive}",
				GameReflectionModel.PrimitiveType.String      => $"{ripe.Primitive}",
				GameReflectionModel.PrimitiveType.Enumeration => $"{ripe.Primitive} • {ripe.Class.AsNotNull()}",
				GameReflectionModel.PrimitiveType.Reference   => $"{ripe.Primitive} • {ripe.Class.AsNotNull()}",
				GameReflectionModel.PrimitiveType.List        => $"{ripe.Primitive} • {GameReflectionHelper.MakeTypeName(ripe.Element.AsNotNull())}",
				GameReflectionModel.PrimitiveType.Map         => $"{ripe.Primitive} • {GameReflectionHelper.MakeTypeName(ripe.Element.AsNotNull())}",
				GameReflectionModel.PrimitiveType.Object      => $"{ripe.Primitive} • {ripe.Class.AsNotNull()}",
				_                                             => throw new (),
			};
		}

		// ----------------

		public static String MakeEnumerationExpression (
			Tuple<String> ripe
		) {
			return ripe.Item1;
		}

		public static Tuple<String> ParseEnumerationExpression (
			String raw
		) {
			var ripe = new Tuple<String>(raw);
			return ripe;
		}

		// ----------------

		public static String MakeReferenceExpression (
			Tuple<String, String> ripe
		) {
			return $"RTID({(ripe.Item1.Length == 0 && ripe.Item2.Length == 0 ? "0" : $"{ripe.Item2}@{ripe.Item1}")})";
		}

		public static Tuple<String, String> ParseReferenceExpression (
			String raw
		) {
			var rawMatch = Regex.Match(raw, @"^RTID\(([0-9a-zA-Z.$]+)[@]([0-9a-zA-Z.$]+)\)$");
			GF.AssertTest(rawMatch.Success);
			var ripe = new Tuple<String, String>(rawMatch.Groups[2].Value, rawMatch.Groups[1].Value);
			return ripe;
		}

		// ----------------

		public static JToken MakeDataValue (
			GameReflectionModel.DescriptorMap descriptor,
			GameReflectionModel.CompositeType type,
			Object                            ripe
		) {
			var raw = default(JToken);
			switch (type.Primitive) {
				case GameReflectionModel.PrimitiveType.Boolean: {
					GF.AssertTest(type is { Element: null, Class: null });
					raw = new JValue(ripe.AsStruct<Boolean>());
					break;
				}
				case GameReflectionModel.PrimitiveType.Integer: {
					GF.AssertTest(type is { Element: null, Class: null });
					raw = new JValue(ripe.AsStruct<Integer>());
					break;
				}
				case GameReflectionModel.PrimitiveType.Floater: {
					GF.AssertTest(type is { Element: null, Class: null });
					raw = new JValue(ripe.AsStruct<Floater>());
					break;
				}
				case GameReflectionModel.PrimitiveType.String: {
					GF.AssertTest(type is { Element: null, Class: null });
					raw = new JValue(ripe.AsClass<String>());
					break;
				}
				case GameReflectionModel.PrimitiveType.Enumeration: {
					GF.AssertTest(type is { Element: null, Class: not null });
					raw = new JValue(GameReflectionHelper.MakeEnumerationExpression(ripe.AsClass<Tuple<String>>()));
					break;
				}
				case GameReflectionModel.PrimitiveType.Reference: {
					GF.AssertTest(type is { Element: null, Class: not null });
					raw = new JValue(GameReflectionHelper.MakeReferenceExpression(ripe.AsClass<Tuple<String, String>>()));
					break;
				}
				case GameReflectionModel.PrimitiveType.List: {
					GF.AssertTest(type is { Element: not null, Class: null });
					var ripeValue = ripe.AsClass<List<GameReflectionModel.FixedValue>>();
					var rawValue = new JArray();
					foreach (var ripeValueItem in ripeValue) {
						rawValue.Add(GameReflectionHelper.MakeDataValue(descriptor, type.Element, ripeValueItem.Value));
					}
					raw = rawValue;
					break;
				}
				case GameReflectionModel.PrimitiveType.Map: {
					GF.AssertTest(type is { Element: not null, Class: null });
					var ripeValue = ripe.AsClass<List<GameReflectionModel.NamedValue>>();
					var rawValue = new JObject();
					foreach (var ripeValueItem in ripeValue) {
						rawValue.Add(ripeValueItem.Name, GameReflectionHelper.MakeDataValue(descriptor, type.Element, ripeValueItem.Value));
					}
					raw = rawValue;
					break;
				}
				case GameReflectionModel.PrimitiveType.Object: {
					GF.AssertTest(type is { Element: null, Class: not null });
					var objectDescriptor = descriptor.Object[type.Class];
					var ripeValue = ripe.AsClass<GameReflectionModel.FixedObject>();
					var rawValue = new JObject();
					GF.AssertTest(ripeValue.Property.Count == objectDescriptor.Count);
					for (var propertyGroupIndex = 0; propertyGroupIndex < objectDescriptor.Count; propertyGroupIndex++) {
						var propertyGroupDescriptor = objectDescriptor[propertyGroupIndex];
						var propertyGroupValue = ripeValue.Property[propertyGroupIndex];
						for (var propertyItemIndex = 0; propertyItemIndex < propertyGroupDescriptor.Property.Count; propertyItemIndex++) {
							var propertyItemDescriptor = propertyGroupDescriptor.Property[propertyItemIndex];
							var propertyItemValue = propertyGroupValue[propertyItemIndex];
							if (propertyItemValue is not null) {
								rawValue.Add(propertyItemDescriptor.Name, GameReflectionHelper.MakeDataValue(descriptor, propertyItemDescriptor.Type, propertyItemValue.Value));
							}
						}
					}
					foreach (var ripeValueExtraProperty in ripeValue.ExtraProperty) {
						rawValue.Add(ripeValueExtraProperty.Name, GameReflectionHelper.MakeDataValue(descriptor, ripeValueExtraProperty.Type, ripeValueExtraProperty.Value));
					}
					raw = rawValue;
					break;
				}
				default: throw new ();
			}
			return raw;
		}

		public static Object ParseDataValue (
			GameReflectionModel.DescriptorMap descriptor,
			GameReflectionModel.CompositeType type,
			JToken                            raw
		) {
			var ripe = default(Object);
			switch (type.Primitive) {
				case GameReflectionModel.PrimitiveType.Boolean: {
					GF.AssertTest(type is { Element: null, Class: null });
					GF.AssertTest(raw.Type == JTokenType.Boolean);
					ripe = raw.Value<Boolean>();
					break;
				}
				case GameReflectionModel.PrimitiveType.Integer: {
					GF.AssertTest(type is { Element: null, Class: null });
					GF.AssertTest(raw.Type == JTokenType.Integer);
					ripe = raw.Value<Integer>();
					break;
				}
				case GameReflectionModel.PrimitiveType.Floater: {
					GF.AssertTest(type is { Element: null, Class: null });
					GF.AssertTest(raw.Type == JTokenType.Float);
					ripe = raw.Value<Floater>();
					break;
				}
				case GameReflectionModel.PrimitiveType.String: {
					GF.AssertTest(type is { Element: null, Class: null });
					GF.AssertTest(raw.Type == JTokenType.String);
					ripe = raw.Value<String>().AsNotNull();
					break;
				}
				case GameReflectionModel.PrimitiveType.Enumeration: {
					GF.AssertTest(type is { Element: null, Class: not null });
					GF.AssertTest(raw.Type == JTokenType.String);
					ripe = GameReflectionHelper.ParseEnumerationExpression(raw.Value<String>().AsNotNull());
					break;
				}
				case GameReflectionModel.PrimitiveType.Reference: {
					GF.AssertTest(type is { Element: null, Class: not null });
					GF.AssertTest(raw.Type == JTokenType.String);
					ripe = GameReflectionHelper.ParseReferenceExpression(raw.Value<String>().AsNotNull());
					break;
				}
				case GameReflectionModel.PrimitiveType.List: {
					GF.AssertTest(type is { Element: not null, Class: null });
					GF.AssertTest(raw.Type == JTokenType.Array);
					var rawValue = raw.Value<JArray>().AsNotNull();
					var ripeValue = new List<GameReflectionModel.FixedValue>(rawValue.Count);
					foreach (var rawValueItem in rawValue) {
						ripeValue.Add(new () { Value = GameReflectionHelper.ParseDataValue(descriptor, type.Element, rawValueItem) });
					}
					ripe = ripeValue;
					break;
				}
				case GameReflectionModel.PrimitiveType.Map: {
					GF.AssertTest(type is { Element: not null, Class: null });
					GF.AssertTest(raw.Type == JTokenType.Object);
					var rawValue = raw.Value<JObject>().AsNotNull();
					var ripeValue = new List<GameReflectionModel.NamedValue>(rawValue.Count);
					foreach (var rawValueItem in rawValue) {
						GF.AssertTest(rawValueItem.Value is not null);
						ripeValue.Add(new () { Name = rawValueItem.Key, Value = GameReflectionHelper.ParseDataValue(descriptor, type.Element, rawValueItem.Value) });
					}
					ripe = ripeValue;
					break;
				}
				case GameReflectionModel.PrimitiveType.Object: {
					GF.AssertTest(type is { Element: null, Class: not null });
					var objectDescriptor = descriptor.Object[type.Class];
					GF.AssertTest(raw.Type == JTokenType.Object);
					var rawValue = raw.Value<JObject>().AsNotNull();
					var ripeValue = new GameReflectionModel.FixedObject() {
						Property = [],
						ExtraProperty = [],
					};
					foreach (var propertyGroupDescriptor in objectDescriptor) {
						var ripePropertyGroup = new List<GameReflectionModel.FixedValue?>(propertyGroupDescriptor.Property.Count);
						foreach (var propertyItemDescriptor in propertyGroupDescriptor.Property) {
							if (rawValue.ContainsKey(propertyItemDescriptor.Name)) {
								ripePropertyGroup.Add(new () { Value = GameReflectionHelper.ParseDataValue(descriptor, propertyItemDescriptor.Type, rawValue[propertyItemDescriptor.Name].AsNotNull()) });
								rawValue.Remove(propertyItemDescriptor.Name);
							}
							else {
								ripePropertyGroup.Add(null);
							}
						}
						ripeValue.Property.Add(ripePropertyGroup);
					}
					// TODO : extra property
					//foreach (var extraItem in sourceValue) {
					//	var extraName = extraItem.Key;
					//	var extraValue = extraItem.Value;
					//	var extraType = (ObjectModel.CompositeType)null!;
					//	destinationValue.ExtraProperty.Add(new ObjectModel.ExtraValue() { Value = extraItem.Value, Name = extraItem.Key, Type =  });
					//}
					ripe = ripeValue;
					break;
				}
				default: throw new ();
			}
			return ripe;
		}

		// ----------------

		public static Object GenerateDataValueDefault (
			GameReflectionModel.DescriptorMap descriptor,
			GameReflectionModel.CompositeType type,
			Boolean                           withElement
		) {
			var value = default(Object);
			switch (type.Primitive) {
				case GameReflectionModel.PrimitiveType.Boolean: {
					GF.AssertTest(type is { Element: null, Class: null });
					value = (Boolean)false;
					break;
				}
				case GameReflectionModel.PrimitiveType.Integer: {
					GF.AssertTest(type is { Element: null, Class: null });
					value = (Integer)0;
					break;
				}
				case GameReflectionModel.PrimitiveType.Floater: {
					GF.AssertTest(type is { Element: null, Class: null });
					value = (Floater)0.0;
					break;
				}
				case GameReflectionModel.PrimitiveType.String: {
					GF.AssertTest(type is { Element: null, Class: null });
					value = (String)"";
					break;
				}
				case GameReflectionModel.PrimitiveType.Enumeration: {
					GF.AssertTest(type is { Element: null, Class: not null });
					value = new Tuple<String>("");
					break;
				}
				case GameReflectionModel.PrimitiveType.Reference: {
					GF.AssertTest(type is { Element: null, Class: not null });
					value = new Tuple<String, String>("", "");
					break;
				}
				case GameReflectionModel.PrimitiveType.List: {
					GF.AssertTest(type is { Element: not null, Class: null });
					value = new List<GameReflectionModel.FixedValue>();
					if (withElement) {
						value.AsClass<List<GameReflectionModel.FixedValue>>().Add(new () { Value = GameReflectionHelper.GenerateDataValueDefault(descriptor, type.Element, withElement) });
					}
					break;
				}
				case GameReflectionModel.PrimitiveType.Map: {
					GF.AssertTest(type is { Element: not null, Class: null });
					value = new List<GameReflectionModel.NamedValue>();
					if (withElement) {
						value.AsClass<List<GameReflectionModel.NamedValue>>().Add(new () { Name = "", Value = GameReflectionHelper.GenerateDataValueDefault(descriptor, type.Element, withElement) });
					}
					break;
				}
				case GameReflectionModel.PrimitiveType.Object: {
					GF.AssertTest(type is { Element: null, Class: not null });
					var objectDescriptor = descriptor.Object[type.Class];
					value = new GameReflectionModel.FixedObject() {
						Property = objectDescriptor.Select((propertyGroupDescriptor) => (propertyGroupDescriptor.Property.Select((propertyItemDescriptor) => (!withElement ? null : new GameReflectionModel.FixedValue() { Value = GameReflectionHelper.GenerateDataValueDefault(descriptor, propertyItemDescriptor.Type, withElement) })).ToList())).ToList(),
						ExtraProperty = [],
					};
					break;
				}
				default: throw new ();
			}
			return value;
		}

		#endregion

		#region compile

		public static void CompileEnumerationDescriptor (
			Dictionary<String, GameReflectionModel.EnumerationDescriptor> ripe,
			GameReflectionModel.EnumerationDescriptor                     raw
		) {
			ripe.Add(raw.Name, raw);
			return;
		}

		public static void CompileObjectDescriptor (
			Dictionary<String, List<GameReflectionModel.ObjectDescriptor>> ripe,
			GameReflectionModel.ObjectDescriptor                           raw,
			List<GameReflectionModel.ObjectDescriptor>                     rawParent
		) {
			var ripeItem = new List<GameReflectionModel.ObjectDescriptor>([..rawParent, raw]);
			ripe.Add(raw.Name, ripeItem);
			foreach (var rawDerived in raw.Derived) {
				GameReflectionHelper.CompileObjectDescriptor(ripe, rawDerived, ripeItem);
			}
			return;
		}

		public static GameReflectionModel.DescriptorMap CompileDescriptorArchive (
			GameReflectionModel.DescriptorArchive raw
		) {
			var ripe = new GameReflectionModel.DescriptorMap() {
				Enumeration = [],
				Object = [],
			};
			foreach (var rawEnumeration in raw.Enumeration) {
				GameReflectionHelper.CompileEnumerationDescriptor(ripe.Enumeration, rawEnumeration);
			}
			foreach (var rawObject in raw.Object) {
				GameReflectionHelper.CompileObjectDescriptor(ripe.Object, rawObject, []);
			}
			return ripe;
		}

		// ----------------

		public static List<GameReflectionModel.AddressedFixedObject> CompileDataArchive (
			GameReflectionModel.DescriptorMap descriptor,
			GameReflectionModel.DataArchive   raw
		) {
			var ripe = new List<GameReflectionModel.AddressedFixedObject>(raw.objects.Count);
			foreach (var rawObject in raw.objects) {
				var objectType = new GameReflectionModel.CompositeType() {
					Primitive = GameReflectionModel.PrimitiveType.Object,
					Element = null,
					Class = rawObject.objclass,
				};
				var ripeObject = GameReflectionHelper.ParseDataValue(descriptor, objectType, rawObject.objdata).AsClass<GameReflectionModel.FixedObject>();
				ripe.Add(new () {
					Alias = rawObject.aliases is { Count: 1 } ? rawObject.aliases[0] : null,
					Class = rawObject.objclass,
					Property = ripeObject.Property,
					ExtraProperty = ripeObject.ExtraProperty,
				});
			}
			return ripe;
		}

		#endregion

		#region load

		public static async Task<GameReflectionModel.DescriptorArchive> LoadDescriptorArchive (
			String file
		) {
			return await JsonHelper.DeserializeFile<GameReflectionModel.DescriptorArchive>(file);
		}

		public static async Task<GameReflectionModel.DataArchive> LoadDataArchive (
			String file
		) {
			return await JsonHelper.DeserializeFile<GameReflectionModel.DataArchive>(file);
		}

		#endregion

	}

}