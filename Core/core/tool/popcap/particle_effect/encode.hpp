#pragma once

#include "core/utility/utility.hpp"
#include "core/tool/popcap/particle_effect/common.hpp"
#include "core/tool/common/byte_stream.hpp"

namespace TwinStar::Core::Tool::PopCap::ParticleEffect {

	template <auto version> requires (check_version(version, {}))
	struct Encode :
		Common<version>,
		CommonByteStreamExchangeForOutput {

		using Common = Common<version>;

		using typename Common::Definition;

		using typename Common::VersionNumber;

		// ----------------

		static auto exchange_effect (
			OByteStreamView &                   data,
			typename Definition::Effect const & value
		) -> Void {
			return;
		}

		// ----------------

		static auto process_whole (
			OByteStreamView &                   data,
			typename Definition::Effect const & definition
		) -> Void {
			throw IncompleteException{};
			return;
		}

		// ----------------

		static auto process (
			OByteStreamView &                   data_,
			typename Definition::Effect const & definition
		) -> Void {
			M_use_zps_of(data);
			return process_whole(data, definition);
		}

	};

}
