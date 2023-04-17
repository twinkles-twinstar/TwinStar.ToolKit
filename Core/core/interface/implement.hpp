#pragma once

#include "core/executor/executor.hpp"
#include <clocale>

namespace TwinStar::Core::Interface::Implement {

	#pragma region function

	inline auto version (
		Size & number
	) -> Void {
		number = mbw<Size>(M_version);
		return;
	}

	inline auto execute (
		Executor::Callback const & callback,
		String const &             script,
		List<String> const &       argument,
		String &                   result
	) -> Void {
		auto script_value = String{};
		if (script.size() >= 1_sz && script.first() == '@'_c) {
			auto script_data = FileSystem::read_file(Path{String{script.tail(script.size() - 1_sz)}});
			script_value.bind(from_byte_view<Character>(script_data.view()));
			script_data.unbind();
		} else {
			script_value = script;
		}
		result = Executor::execute(callback, script_value, argument);
		return;
	}

	inline auto prepare (
	) -> Void {
		std::setlocale(LC_ALL, "C");
		return;
	}

	#pragma endregion

}
