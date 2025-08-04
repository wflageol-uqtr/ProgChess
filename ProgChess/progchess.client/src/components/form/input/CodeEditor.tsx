import CodeMirror, { EditorView, type Extension } from "@uiw/react-codemirror";
import "@uiw/codemirror-theme-dracula";
import { dracula } from "@uiw/codemirror-theme-dracula";
import { langs } from "@uiw/codemirror-extensions-langs";
import { linter, type Diagnostic } from "@codemirror/lint";
import { lintGutter } from "@codemirror/lint";
import type { EditorError } from "../../../utils/type";

interface CodeEditorProps {
  placeholder?: string;
  value: string;
  height: number;
  onChange: (value: string) => void;
  error?: any;
  editable?: boolean;
  executionError?: EditorError;
}

export default function CodeEditor({
  placeholder,
  value,
  height,
  onChange,
  error,
  editable = true,
  executionError,
}: CodeEditorProps) {
  const linterExtension = errorLineLinter(executionError?.error ?? "");

  function errorLineLinter(error?: string): Extension {
    return linter((view) => {
      const diagnostics: Diagnostic[] = [];

      if (executionError?.line) {
        const line = view.state.doc.line(executionError?.line);
        diagnostics.push({
          from: line.from,
          to: line.to,
          severity: "error",
          message: error ?? "Erreur est survenue",
          actions: [
            {
              name: "Explain",
              apply(view, from, to) {
                alert(error ?? "Erreur est survenue");
              },
            },
          ],
        });
      }

      return diagnostics;
    });
  }

  return (
    <div className="h-full overflow-x-auto">
      <CodeMirror
        className={error ? "border border-red-500 rounded-lg" : ""}
        placeholder={placeholder}
        value={value}
        theme={dracula}
        height={`${height}px`}
        extensions={[
          langs.typescript(),
          langs.javascript(),
          linterExtension,
          lintGutter(),
          // EditorView.lineWrapping,
        ]}
        onChange={onChange}
        editable={editable}
      />
    </div>
  );
}
