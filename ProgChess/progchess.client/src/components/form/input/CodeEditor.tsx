import CodeMirror from "@uiw/react-codemirror";
import "@uiw/codemirror-theme-dracula";
import { dracula } from "@uiw/codemirror-theme-dracula";
import { langs } from "@uiw/codemirror-extensions-langs";
import { useLayoutEffect, useRef, useState } from "react";

interface CodeEditorProps {
  placeholder?: string;
  value: string;
  onChange: (value: string) => void;
}

export default function CodeEditor({
  placeholder,
  value,
  onChange,
}: CodeEditorProps) {
  const [height, setHeight] = useState<number>(0);
  const parentRef = useRef<HTMLDivElement>(null);

  useLayoutEffect(() => {
    setHeight(parentRef.current?.offsetHeight!);
  });

  return (
    <div ref={parentRef} className="h-full">
      <CodeMirror
        placeholder={placeholder}
        value={value}
        theme={dracula}
        height={`${height}px`}
        extensions={[langs.python()]}
        onChange={onChange}
      />
    </div>
  );
}
