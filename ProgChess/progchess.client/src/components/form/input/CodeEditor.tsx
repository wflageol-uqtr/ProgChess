import CodeMirror from "@uiw/react-codemirror";
import "@uiw/codemirror-theme-dracula";
import { dracula } from "@uiw/codemirror-theme-dracula";
import { langs } from "@uiw/codemirror-extensions-langs";

interface CodeEditorProps {
  placeholder?: string;
  value: string;
  height: number;
  onChange: (value: string) => void;
  error?: any;
}

export default function CodeEditor({
  placeholder,
  value,
  height,
  onChange,
  error,
}: CodeEditorProps) {
  return (
    <div className="h-full">
      <CodeMirror
        className={error ? "border border-red-500 rounded-lg" : ""}
        placeholder={placeholder}
        value={value}
        theme={dracula}
        height={`${height}px`}
        extensions={[langs.typescript(), langs.javascript()]}
        onChange={onChange}
      />
    </div>
  );
}
