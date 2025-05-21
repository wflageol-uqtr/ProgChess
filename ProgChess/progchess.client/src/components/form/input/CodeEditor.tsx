import CodeMirror from "@uiw/react-codemirror";
import "@uiw/codemirror-theme-dracula";
import { dracula } from "@uiw/codemirror-theme-dracula";
import { langs } from "@uiw/codemirror-extensions-langs";

interface CodeEditorProps {
  placeholder: string;
  value: string;
  onChange: (value: string) => void;
}

export default function CodeEditor({
  placeholder,
  value,
  onChange,
}: CodeEditorProps) {
  return (
    <CodeMirror
      placeholder={placeholder}
      value={value}
      theme={dracula}
      height="350px"
      extensions={[langs.python()]}
      onChange={(val: string) => {
        onChange(val);
      }}
    />
  );
}
