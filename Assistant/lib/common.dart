import 'dart:core' as core;
import 'package:material_symbols_icons/material_symbols_icons.dart';

// ----------------

typedef Void = void;
typedef Boolean = core.bool;
typedef Integer = core.int;
typedef Floater = core.double;
typedef String = core.String;

typedef IconSymbols = Symbols;

// ----------------

const String kApplicationName = 'TwinStar ToolKit - Assistant';

const String kApplicationVersion = '38';

// ----------------

Void assertTest(
  Boolean condition,
) {
  if (!condition) {
    throw core.AssertionError();
  }
  return;
}
