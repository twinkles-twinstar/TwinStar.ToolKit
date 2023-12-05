namespace TwinStar.Script.Support.PvZ2.PackageProject {

	// ------------------------------------------------

	export type ResourceCategory = {
		resolution: null | bigint;
		locale: null | string;
	};

	const ResourceTypeX = [
		'dummy',
		'general',
		'texture',
		'special_rton',
		'special_ptx',
		'special_pam',
		'special_wem',
	] as const;

	export type ResourceType = typeof ResourceTypeX[number];

	export const ResourceTypeE = ResourceTypeX as unknown as ResourceType[];

	export type DummyResourceProperty = {
	};

	export type GeneralResourceProperty = {
		path: string;
		type: RegularResourceManifest.ResourceType;
	};

	export type TextureResourceProperty = {
		path: string;
		format: bigint;
		pitch: bigint;
		additional_byte_count: bigint;
		size: [bigint, bigint];
		sprite: Array<RegularResourceManifest.TextureSpriteResource>;
	};

	export type SpecialRTONResourceProperty = {
		conversion: string;
		path: string;
	};

	export type SpecialPTXResourceProperty = {
		conversion: string;
		path: string;
		resolution: bigint;
		sprite: Array<{
			source: string;
			id: string;
			path: string;
			offset: [bigint, bigint];
			separate: [bigint, bigint];
		}>;
	};

	export type SpecialPAMResourceProperty = {
		conversion: string;
		path: string;
	};

	export type SpecialPOPFXPesourceProperty = {
		conversion: string;
		path: string;
	};

	export type SpecialWEMResourceProperty = {
		conversion: string;
		path: string;
	};

	export type SpecialBNKResourceProperty = {
		conversion: string;
		path: string;
	};

	export type ResourceProperty = DummyResourceProperty | GeneralResourceProperty | TextureResourceProperty | SpecialRTONResourceProperty | SpecialPTXResourceProperty | SpecialPAMResourceProperty | SpecialPOPFXPesourceProperty | SpecialWEMResourceProperty | SpecialBNKResourceProperty;

	// ------------------------------------------------

	export type Variable = {
		name: string;
		value: string;
	};

	export type ConversionSetting = {
		rton: Array<{
			name: string;
			version: typeof Kernel.Tool.PopCap.ReflectionObjectNotation.Version.Value;
		}>;
		ptx: Array<{
			name: string;
			format: Support.PopCap.Texture.Encoding.Format;
			index: bigint;
		}>;
		pam: Array<{
			name: string;
			version: typeof Kernel.Tool.PopCap.Animation.Version.Value;
		}>;
		wem: Array<{
			name: string;
			format: Support.Wwise.Media.Encode.Format;
		}>;
	};

	// ------------------------------------------------

	const ManifestTypeX = [
		'internal',
		'external_rton_with_array_path',
		'external_rton_with_string_path',
		'external_newton',
	] as const;

	export type ManifestType = typeof ManifestTypeX[number];

	export const ManifestTypeE = ManifestTypeX as unknown as ManifestType[];

	// ------------------------------------------------

	export type ResourceSetting = {
		category: ResourceCategory;
		type: ResourceType;
		property: ResourceProperty;
		variable: Array<Variable>;
	};

	export type GroupSetting = {
		variable: Array<Variable>;
	};

	export type PartSetting = {
		variable: Array<Variable>;
	};

	export type PackageSetting = {
		name: string;
		part: Array<string>;
		version: {
			number: 1n | 3n | 4n;
			extended_texture_information_for_pvz2_cn: 0n | 1n | 2n | 3n;
		};
		compression: {
			general: boolean;
			texture: boolean;
			filter: Array<string>;
		};
		manifest: {
			type: ManifestType;
			suffix: string;
		};
		category: {
			resolution: Array<bigint>;
			locale: Array<string>;
		};
		conversion: ConversionSetting;
		variable: Array<Variable>;
	};

	export type ProjectSetting = {
		package: Array<PackageSetting>;
	};

	// ------------------------------------------------

}