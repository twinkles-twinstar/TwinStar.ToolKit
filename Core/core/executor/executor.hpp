#pragma once

#include "core/utility/utility.hpp"
#include "core/executor/callback.hpp"
#include "core/executor/context.hpp"
#include "core/executor/interface.hpp"

namespace TwinStar::Core::Executor {

	#pragma region function

	inline auto execute (
		Callback const &     callback,
		String const &       script,
		List<String> const & argument
	) -> Optional<String> {
		auto guard = std::lock_guard{JS::g_mutex};
		auto context = Context{callback};
		Interface::inject(context);
		auto script_result = context.context().evaluate(script).call(make_list<JS::Value>(context.context().new_value(argument)));
		auto result = Optional<String>{};
		if (script_result.is_null()) {
			result = k_null_optional;
		} else if (script_result.is_string()) {
			result = make_optional_of(script_result.get_string());
		} else if (script_result.is_exception()) {
			result = make_optional_of(context.context().evaluate("((error) => (`${error}`))"_sv).call(make_list<JS::Value>(context.context().current_exception())).get_string());
		} else {
			result = make_optional_of("invalid return type"_s);
		}
		return result;
	}

	#pragma endregion

}
