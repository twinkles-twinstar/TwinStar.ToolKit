namespace TwinStar.Script.Entry.method.js {

	// ------------------------------------------------

	// execute

	type Configuration = {
	};

	export function _injector(
		configuration: Configuration,
	) {
		g_executor_method.push(
			Executor.method_of({
				id: 'js.execute',
				name(
				) {
					return Executor.query_method_name(this.id);
				},
				worker(a: Entry.CommonArgument & {
					script_file: Executor.Argument<string, false>;
					is_module: Executor.Argument<boolean, false>;
				}) {
					let script_file: string;
					let is_module: boolean;
					{
						script_file = Executor.request_argument(
							Executor.query_argument_name(this.id, 'script_file'),
							a.script_file,
							(value) => (value),
							null,
							(initial) => (Console.path('file', ['in'], null, null, initial)),
						);
						is_module = Executor.request_argument(
							Executor.query_argument_name(this.id, 'is_module'),
							a.is_module,
							(value) => (value),
							null,
							(initial) => (Console.confirmation(null, null, initial)),
						);
					}
					let result = KernelX.Miscellaneous.evaluate_fs(script_file, is_module);
					return [`${result}`];
				},
				default_argument: {
					...Entry.g_common_argument,
					script_file: '?input',
					is_module: '?input',
				},
				input_filter: Entry.file_system_path_test_generator([['file', /.+(\.js)$/i]]),
				input_forwarder: 'script_file',
			}),
		);
		g_executor_method_of_batch.push(
		);
	}

	// ------------------------------------------------

}

({
	injector: TwinStar.Script.Entry.method.js._injector,
});