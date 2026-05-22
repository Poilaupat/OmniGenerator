# DotLiquid Custom Filters

This document describes the custom filters available for use in composite field templates.

## Overview

Composite fields use DotLiquid templates to format values. Custom filters extend the functionality beyond the standard DotLiquid filters.

## Available Custom Filters

### `format`

Formats a value using standard .NET format strings.

**Syntax:** `{{ value | format:"format_string" }}`

**Examples:**
```liquid
{{packet_number | format:"D4"}}          ? "0042" (for input 42)
{{amount | format:"F2"}}                 ? "123.45" (for input 123.45)
{{date | format:"yyyy-MM-dd"}}           ? "2024-01-15"
{{percentage | format:"P2"}}             ? "75.50%" (for input 0.755)
```

**Common Format Strings:**
- `D4`, `D6`, etc. - Decimal with leading zeros (4, 6 digits)
- `F2` - Fixed-point with 2 decimal places
- `N0` - Number with thousands separator, no decimals
- `C` - Currency
- `P` - Percentage

### `pad_left`

Pads a value to the left with a specified character (default: "0").

**Syntax:** `{{ value | pad_left:length }}` or `{{ value | pad_left:length,'char' }}`

**Examples:**
```liquid
{{packet_number | pad_left:4}}           ? "0042" (for input 42)
{{id | pad_left:6}}                      ? "000123" (for input 123)
{{code | pad_left:5,'X'}}                ? "XXX42" (for input "42")
```

### `pad_right`

Pads a value to the right with a specified character (default: " ").

**Syntax:** `{{ value | pad_right:length }}` or `{{ value | pad_right:length,'char' }}`

**Examples:**
```liquid
{{name | pad_right:20}}                  ? "John                " (for input "John")
{{code | pad_right:10,'0'}}              ? "ABC0000000" (for input "ABC")
```

## Usage in Configuration

### Example: Packet Name Generation

```json
{
  "$type": "composite",
  "name": "packet-name",
  "dependent-upon": "packet-date,deposit-bank-code,agency-code,scanner-code,packet-number",
  "format": "{{packet_date | date:\"yyMMddHHmmss\"}}{{deposit_bank_code}}{{agency_code}}{{scanner_code}}{{packet_number | pad_left:4}}"
}
```

**Result:** `2401151430001600316001001` + `0042`

### Example: Formatted Invoice Line

```json
{
  "$type": "composite",
  "name": "invoice-line",
  "dependent-upon": "item-code,description,quantity,unit-price",
  "format": "{{item_code | pad_right:10}} {{description | pad_right:30}} {{quantity | format:\"D3\"}} {{unit_price | format:\"F2\"}}"
}
```

**Result:** `ABC123     Widget Tool 123           005 12.50`

## Combining Filters

You can chain multiple filters together:

```liquid
{{value | upcase | pad_right:20}}
{{number | format:"F2" | prepend:"$"}}
```

## Standard DotLiquid Filters

In addition to custom filters, all standard DotLiquid filters are available:

- `date` - Format dates
- `upcase` / `downcase` - Change case
- `capitalize` - Capitalize first letter
- `prepend` / `append` - Add text before/after
- `replace` - Replace text
- `slice` - Extract substring
- And many more...

See [DotLiquid documentation](https://github.com/dotliquid/dotliquid/wiki/DotLiquid-for-Designers) for the complete list.

## Implementation Notes

### Method Naming Convention

C# method names in `LiquidCustomFilters.cs` are automatically converted to snake_case:

| C# Method | Template Filter |
|-----------|----------------|
| `PadLeft` | `pad_left` |
| `Format` | `format` |
| `PadRight` | `pad_right` |
| `MyCustomMethod` | `my_custom_method` |

### Adding New Filters

To add a new custom filter:

1. Add a `public static` method to `LiquidCustomFilters.cs`
2. First parameter is the input value
3. Additional parameters become filter arguments
4. The method is automatically registered at startup

Example:

```csharp
public static string Truncate(string input, int length, string suffix = "...")
{
    if (input == null || input.Length <= length) return input;
    return input.Substring(0, length - suffix.Length) + suffix;
}
```

Usage:
```liquid
{{description | truncate:50}}
{{description | truncate:50,'…'}}
```
