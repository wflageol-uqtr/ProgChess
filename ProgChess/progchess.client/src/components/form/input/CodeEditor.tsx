import CodeMirror from "@uiw/react-codemirror";
import "@uiw/codemirror-theme-dracula";
import { dracula } from "@uiw/codemirror-theme-dracula";
import { langs } from "@uiw/codemirror-extensions-langs";

interface CodeEditorProps {
  placeholder?: string;
  value: string;
  height: number;
  onChange: (value: string) => void;
}

export default function CodeEditor({
  placeholder,
  value,
  height,
  onChange,
}: CodeEditorProps) {
  return (
    <div className="h-full">
      <CodeMirror
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
