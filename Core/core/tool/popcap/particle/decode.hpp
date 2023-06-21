#pragma once

#include "core/utility/utility.hpp"
#include "core/tool/popcap/particle/common.hpp"
#include "core/tool/common/byte_stream.hpp"

namespace TwinStar::Core::Tool::PopCap::Particle {

	template <auto version> requires (check_version(version, {}, {}))
	struct Decode :
		Common<version>,
		CommonByteStreamExchangeForInput {

		using Common = Common<version>;

		using typename Common::Definition;

		using typename Common::MagicIdentifier;

		using Common::k_magic_identifier;

		using typename Common::IntegerOfPlatform;

		using Common::k_field_data_size;

		using Common::k_emitter_data_size;

		// ----------------

		static auto exchange_track_node_list (
			IByteStreamView &                      data,
			List<typename Definition::TrackNode> & value_list
		) -> Void {
			auto ignored = Integer{0_i};
			exchange_list_size(data, value_list, &exchange_size_fixed<IntegerU32>);
			for (auto & value : value_list) {
				exchange_floater_fixed<FloaterS32>(data, value.time);
				exchange_floater_fixed<FloaterS32>(data, value.low_value);
				exchange_floater_fixed<FloaterS32>(data, value.high_value);
				exchange_integer_fixed<IntegerU32>(data, value.curve);
				exchange_integer_fixed<IntegerU32>(data, value.distribution);
			}
			return;
		}

		static auto exchange_field_list (
			IByteStreamView &                  data,
			List<typename Definition::Field> & value_list
		) -> Void {
			auto ignored = Integer{0_i};
			exchange_raw_constant(data, cbw<IntegerU32>(k_field_data_size));
			for (auto & value : value_list) {
				exchange_integer_fixed<IntegerOfPlatform>(data, value.type);
				exchange_integer_fixed<IntegerOfPlatform>(data, ignored);
				exchange_integer_fixed<IntegerOfPlatform>(data, ignored);
				exchange_integer_fixed<IntegerOfPlatform>(data, ignored);
				exchange_integer_fixed<IntegerOfPlatform>(data, ignored);
			}
			for (auto & value : value_list) {
				exchange_track_node_list(data, value.offset_x);
				exchange_track_node_list(data, value.offset_y);
			}
			return;
		}

		static auto exchange_emitter_list (
			IByteStreamView &                    data,
			List<typename Definition::Emitter> & value_list
		) -> Void {
			auto ignored = Integer{0_i};
			exchange_list_size(data, value_list, &exchange_size_fixed<IntegerU32>);
			if constexpr (check_version(version, {}, {true})) {
				exchange_integer_fixed<IntegerU32>(data, ignored);
			}
			exchange_raw_constant(data, cbw<IntegerU32>(k_emitter_data_size));
			for (auto & value : value_list) {
				exchange_integer_fixed<IntegerOfPlatform>(data, ignored);
				exchange_integer_fixed<IntegerU32>(data, value.image_column);
				exchange_integer_fixed<IntegerU32>(data, value.image_row);
				exchange_integer_fixed<IntegerU32>(data, value.image_frame);
				exchange_integer_fixed<IntegerU32>(data, value.animated);
				exchange_integer_fixed<IntegerU32>(data, value.flag);
				exchange_integer_fixed<IntegerU32>(data, value.type);
				exchange_integer_fixed<IntegerOfPlatform>(data, ignored);
				exchange_integer_fixed<IntegerOfPlatform>(data, ignored);
				for (auto & index : SizeRange{22_sz}) {
					exchange_integer_fixed<IntegerOfPlatform>(data, ignored);
					exchange_integer_fixed<IntegerOfPlatform>(data, ignored);
				}
				exchange_integer_fixed<IntegerOfPlatform>(data, ignored);
				exchange_list_size(data, value.field, &exchange_size_fixed<IntegerOfPlatform>);
				exchange_integer_fixed<IntegerOfPlatform>(data, ignored);
				exchange_list_size(data, value.system_field, &exchange_size_fixed<IntegerOfPlatform>);
				for (auto & index : SizeRange{16_sz}) {
					exchange_integer_fixed<IntegerOfPlatform>(data, ignored);
					exchange_integer_fixed<IntegerOfPlatform>(data, ignored);
				}
			}
			for (auto & value : value_list) {
				if constexpr (check_version(version, {VersionPlatform::Constant::desktop(), VersionPlatform::Constant::television()}, {})) {
					exchange_string_block<IntegerU32>(data, value.image);
				}
				if constexpr (check_version(version, {VersionPlatform::Constant::mobile()}, {})) {
					exchange_integer_fixed<IntegerU32>(data, value.image);
				}
				if constexpr (check_version(version, {VersionPlatform::Constant::television()}, {})) {
					exchange_string_block<IntegerU32>(data, value.image_path);
				}
				exchange_string_block<IntegerU32>(data, value.name);
				exchange_track_node_list(data, value.system_duration);
				exchange_string_block<IntegerU32>(data, value.on_duration);
				exchange_track_node_list(data, value.cross_fade_duration);
				exchange_track_node_list(data, value.spawn_rate);
				exchange_track_node_list(data, value.spawn_minimum_active);
				exchange_track_node_list(data, value.spawn_maximum_active);
				exchange_track_node_list(data, value.spawn_maximum_launched);
				exchange_track_node_list(data, value.emitter_radius);
				exchange_track_node_list(data, value.emitter_offset_x);
				exchange_track_node_list(data, value.emitter_offset_y);
				exchange_track_node_list(data, value.emitter_box_x);
				exchange_track_node_list(data, value.emitter_box_y);
				exchange_track_node_list(data, value.emitter_path);
				exchange_track_node_list(data, value.emitter_skew_x);
				exchange_track_node_list(data, value.emitter_skew_y);
				exchange_track_node_list(data, value.particle_duration);
				exchange_track_node_list(data, value.system_red);
				exchange_track_node_list(data, value.system_green);
				exchange_track_node_list(data, value.system_blue);
				exchange_track_node_list(data, value.system_alpha);
				exchange_track_node_list(data, value.system_brightness);
				exchange_track_node_list(data, value.launch_speed);
				exchange_track_node_list(data, value.launch_angle);
				exchange_field_list(data, value.field);
				exchange_field_list(data, value.system_field);
				exchange_track_node_list(data, value.particle_red);
				exchange_track_node_list(data, value.particle_green);
				exchange_track_node_list(data, value.particle_blue);
				exchange_track_node_list(data, value.particle_alpha);
				exchange_track_node_list(data, value.particle_brightness);
				exchange_track_node_list(data, value.particle_spin_angle);
				exchange_track_node_list(data, value.particle_spin_speed);
				exchange_track_node_list(data, value.particle_scale);
				exchange_track_node_list(data, value.particle_stretch);
				exchange_track_node_list(data, value.collision_reflect);
				exchange_track_node_list(data, value.collision_spin);
				exchange_track_node_list(data, value.clip_top);
				exchange_track_node_list(data, value.clip_bottom);
				exchange_track_node_list(data, value.clip_left);
				exchange_track_node_list(data, value.clip_right);
				exchange_track_node_list(data, value.animation_rate);
			}
			return;
		}

		static auto exchange_particle (
			IByteStreamView &               data,
			typename Definition::Particle & value
		) -> Void {
			auto ignored = Integer{0_i};
			exchange_integer_fixed<IntegerOfPlatform>(data, ignored);
			exchange_emitter_list(data, value.emitter);
			return;
		}

		// ----------------

		static auto process_whole (
			IByteStreamView &               data,
			typename Definition::Particle & definition
		) -> Void {
			data.read_constant(k_magic_identifier);
			exchange_particle(data, definition);
			return;
		}

		// ----------------

		static auto process (
			IByteStreamView &               data_,
			typename Definition::Particle & definition
		) -> Void {
			M_use_zps_of(data);
			restruct(definition);
			return process_whole(data, definition);
		}

	};

}
