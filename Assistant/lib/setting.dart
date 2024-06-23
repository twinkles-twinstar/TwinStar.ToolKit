import '/common.dart';
import '/module.dart';
import '/utility/storage_helper.dart';
import '/utility/json_helper.dart';
import '/utility/font_helper.dart';
import '/view/modding_worker/setting.dart' as modding_worker;
import '/view/command_sender/setting.dart' as command_sender;
import '/view/resource_forwarder/setting.dart' as resource_forwarder;
import 'package:flutter/material.dart';

// ----------------

class SettingData {
  String                     mVersion;
  ThemeMode                  mThemeMode;
  Boolean                    mThemeColorState;
  Color                      mThemeColorLight;
  Color                      mThemeColorDark;
  Boolean                    mThemeFontState;
  List<String>               mThemeFontPath;
  Boolean                    mWindowPositionState;
  Integer                    mWindowPositionX;
  Integer                    mWindowPositionY;
  Boolean                    mWindowSizeState;
  Integer                    mWindowSizeWidth;
  Integer                    mWindowSizeHeight;
  String                     mFallbackDirectory;
  ModuleLauncherSetting      mModuleLauncher;
  modding_worker.Setting     mModdingWorker;
  command_sender.Setting     mCommandSender;
  resource_forwarder.Setting mResourceForwarder;
  SettingData({
    required this.mVersion,
    required this.mThemeMode,
    required this.mThemeColorState,
    required this.mThemeColorLight,
    required this.mThemeColorDark,
    required this.mThemeFontState,
    required this.mThemeFontPath,
    required this.mWindowPositionState,
    required this.mWindowPositionX,
    required this.mWindowPositionY,
    required this.mWindowSizeState,
    required this.mWindowSizeWidth,
    required this.mWindowSizeHeight,
    required this.mFallbackDirectory,
    required this.mModuleLauncher,
    required this.mModdingWorker,
    required this.mCommandSender,
    required this.mResourceForwarder,
  });
}

class SettingState {
  GlobalKey<NavigatorState>                           mApplicationNavigatorKey;
  List<String>                                        mThemeFontFamliy;
  Future<Void> Function()?                            mHomeShowLauncherPanel;
  Future<Void> Function(ModuleLauncherConfiguration)? mHomeInsertTabItem;
  List<String>                                        mModdingWorkerMessageFontFamily;
  SettingState({
    required this.mApplicationNavigatorKey,
    required this.mThemeFontFamliy,
    required this.mHomeShowLauncherPanel,
    required this.mHomeInsertTabItem,
    required this.mModdingWorkerMessageFontFamily,
  });
}

class SettingProvider with ChangeNotifier {

  // #region structor

  SettingData data;

  SettingState state;

  // ----------------

  SettingProvider(
  ) :
    data = _createDefaultData(),
    state = _createDefaultState();

  // #endregion

  // #region action

  Future<Void> reset(
  ) async {
    this.data = _createDefaultData();
    this.state = _createDefaultState();
    return;
  }

  Future<Void> apply(
  ) async {
    this.state.mThemeFontFamliy.clear();
    for (var index = 0; index < (!this.data.mThemeFontState ? 0 : this.data.mThemeFontPath.length); index++) {
      var family = await FontHelper.loadFile(this.data.mThemeFontPath[index]);
      if (family != null && !this.state.mThemeFontFamliy.contains(family)) {
        this.state.mThemeFontFamliy.add(family);
      }
    }
    this.state.mModdingWorkerMessageFontFamily.clear();
    for (var index = 0; index < this.data.mModdingWorker.mMessageFont.length; index++) {
      var family = await FontHelper.loadFile(this.data.mModdingWorker.mMessageFont[index]);
      if (family != null && !this.state.mModdingWorkerMessageFontFamily.contains(family)) {
        this.state.mModdingWorkerMessageFontFamily.add(family);
      }
    }
    this.notifyListeners();
    return;
  }

  // #endregion

  // #region storage

  Future<String> get file async {
    return '${await StorageHelper.queryApplicationSharedDirectory()}/setting.json';
  }

  // ----------------

  Future<Void> load({
    String? file = null,
  }) async {
    file ??= await this.file;
    this.data = _parseDataFromJson(await JsonHelper.deserializeFile(file));
    return;
  }

  Future<Void> save({
    String? file = null,
    Boolean apply = true,
  }) async {
    file ??= await this.file;
    if (apply) {
      await this.apply();
    }
    await JsonHelper.serializeFile(file, _makeDataToJson(this.data));
    return;
  }

  // #endregion

  // #region utility

  static SettingData _createDefaultData(
  ) => SettingData(
    mVersion: kApplicationVersion,
    mThemeMode: ThemeMode.system,
    mThemeColorState: false,
    mThemeColorLight: const Color(0xff6200ee),
    mThemeColorDark: const Color(0xffbb86fc),
    mThemeFontState: false,
    mThemeFontPath: [],
    mWindowPositionState: false,
    mWindowPositionX: 0,
    mWindowPositionY: 0,
    mWindowSizeState: false,
    mWindowSizeWidth: 0,
    mWindowSizeHeight: 0,
    mFallbackDirectory: '',
    mModuleLauncher: ModuleLauncherSetting(
      mModule: ModuleType.values.map((e) => ModuleLauncherConfiguration(title: ModuleHelper.query(e).name, type: e, option: [])).toList(),
      mPinned: [],
      mRecent: [],
    ),
    mModdingWorker: modding_worker.Setting(
      mKernel: '',
      mScript: '',
      mArgument: [],
      mImmediateLaunch: true,
      mMessageFont: [],
    ),
    mCommandSender: command_sender.Setting(
      mMethodConfiguration: '',
      mParallelForward: false,
    ),
    mResourceForwarder: resource_forwarder.Setting(
      mOptionConfiguration: '',
      mParallelForward: false,
      mEnableFilter: true,
      mEnableBatch: false,
    ),
  );

  static SettingState _createDefaultState(
  ) => SettingState(
    mApplicationNavigatorKey: GlobalKey<NavigatorState>(),
    mThemeFontFamliy: [],
    mHomeShowLauncherPanel: null,
    mHomeInsertTabItem: null,
    mModdingWorkerMessageFontFamily: [],
  );

  // ----------------

  static dynamic _makeDataToJson(
    SettingData data,
  ) {
    return {
      'version': data.mVersion.selfAlso((it) { assertTest(it == kApplicationVersion); }),
      'theme_mode': data.mThemeMode.name,
      'theme_color_state': data.mThemeColorState,
      'theme_color_light': data.mThemeColorLight.value,
      'theme_color_dark': data.mThemeColorDark.value,
      'theme_font_state': data.mThemeFontState,
      'theme_font_path': data.mThemeFontPath,
      'window_position_state': data.mWindowPositionState,
      'window_position_x': data.mWindowPositionX,
      'window_position_y': data.mWindowPositionY,
      'window_size_state': data.mWindowSizeState,
      'window_size_width': data.mWindowSizeWidth,
      'window_size_height': data.mWindowSizeHeight,
      'fallback_directory': data.mFallbackDirectory,
      'module_launcher': {
        'module': data.mModuleLauncher.mModule.map((dataItem) => {
          'title': dataItem.title,
          'type': dataItem.type.name,
          'option': dataItem.option,
        }).toList(),
        'pinned': data.mModuleLauncher.mPinned.map((dataItem) => {
          'title': dataItem.title,
          'type': dataItem.type.name,
          'option': dataItem.option,
        }).toList(),
        'recent': data.mModuleLauncher.mRecent.map((dataItem) => {
          'title': dataItem.title,
          'type': dataItem.type.name,
          'option': dataItem.option,
        }).toList(),
      },
      'modding_worker': {
        'kernel': data.mModdingWorker.mKernel,
        'script': data.mModdingWorker.mScript,
        'argument': data.mModdingWorker.mArgument,
        'immediate_launch': data.mModdingWorker.mImmediateLaunch,
        'message_font': data.mModdingWorker.mMessageFont,
      },
      'command_sender': {
        'method_configuration': data.mCommandSender.mMethodConfiguration,
        'parallel_forward': data.mCommandSender.mParallelForward,
      },
      'resource_forwarder': {
        'option_configuration': data.mResourceForwarder.mOptionConfiguration,
        'parallel_forward': data.mResourceForwarder.mParallelForward,
        'enable_filter': data.mResourceForwarder.mEnableFilter,
        'enable_batch': data.mResourceForwarder.mEnableBatch,
      },
    };
  }

  static SettingData _parseDataFromJson(
    dynamic json,
  ) {
    return SettingData(
      mVersion: (json['version'] as String).selfAlso((it) { assertTest(it == kApplicationVersion); }),
      mThemeMode: (json['theme_mode'] as String).selfLet((it) => ThemeMode.values.byName(it)),
      mThemeColorState: (json['theme_color_state'] as Boolean),
      mThemeColorLight: (json['theme_color_light'] as Integer).selfLet((it) => Color(it)),
      mThemeColorDark: (json['theme_color_dark'] as Integer).selfLet((it) => Color(it)),
      mThemeFontState: (json['theme_font_state'] as Boolean),
      mThemeFontPath: (json['theme_font_path'] as List<dynamic>).cast<String>(),
      mWindowPositionState: (json['window_position_state'] as Boolean),
      mWindowPositionX: (json['window_position_x'] as Integer),
      mWindowPositionY: (json['window_position_y'] as Integer),
      mWindowSizeState: (json['window_size_state'] as Boolean),
      mWindowSizeWidth: (json['window_size_width'] as Integer),
      mWindowSizeHeight: (json['window_size_height'] as Integer),
      mFallbackDirectory: (json['fallback_directory'] as String),
      mModuleLauncher: (json['module_launcher'] as Map<dynamic, dynamic>).selfLet((jsonPart) => ModuleLauncherSetting(
        mModule: (jsonPart['module'] as List<dynamic>).cast<Map<dynamic, dynamic>>().map((jsonItem) => ModuleLauncherConfiguration(
          title: (jsonItem['title'] as String),
          type: (jsonItem['type'] as String).selfLet((it) => ModuleType.values.byName(it)),
          option: (jsonItem['option'] as List<dynamic>).cast<String>(),
        )).toList(),
        mPinned: (jsonPart['pinned'] as List<dynamic>).cast<Map<dynamic, dynamic>>().map((jsonItem) => ModuleLauncherConfiguration(
          title: (jsonItem['title'] as String),
          type: (jsonItem['type'] as String).selfLet((it) => ModuleType.values.byName(it)),
          option: (jsonItem['option'] as List<dynamic>).cast<String>(),
        )).toList(),
        mRecent: (jsonPart['recent'] as List<dynamic>).cast<Map<dynamic, dynamic>>().map((jsonItem) => ModuleLauncherConfiguration(
          title: (jsonItem['title'] as String),
          type: (jsonItem['type'] as String).selfLet((it) => ModuleType.values.byName(it)),
          option: (jsonItem['option'] as List<dynamic>).cast<String>(),
        )).toList(),
      )),
      mModdingWorker: (json['modding_worker'] as Map<dynamic, dynamic>).selfLet((jsonPart) => modding_worker.Setting(
        mKernel: (jsonPart['kernel'] as String),
        mScript: (jsonPart['script'] as String),
        mArgument: (jsonPart['argument'] as List<dynamic>).cast<String>(),
        mImmediateLaunch: (jsonPart['immediate_launch'] as Boolean),
        mMessageFont: (jsonPart['message_font'] as List<dynamic>).cast<String>(),
      )),
      mCommandSender: (json['command_sender'] as Map<dynamic, dynamic>).selfLet((jsonPart) => command_sender.Setting(
        mMethodConfiguration: (jsonPart['method_configuration'] as String),
        mParallelForward: (jsonPart['parallel_forward'] as Boolean),
      )),
      mResourceForwarder: (json['resource_forwarder'] as Map<dynamic, dynamic>).selfLet((jsonPart) => resource_forwarder.Setting(
        mOptionConfiguration: (jsonPart['option_configuration'] as String),
        mParallelForward: (jsonPart['parallel_forward'] as Boolean),
        mEnableFilter: (jsonPart['enable_filter'] as Boolean),
        mEnableBatch: (jsonPart['enable_batch'] as Boolean),
      )),
    );
  }

  // #endregion

}
