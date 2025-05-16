import ReactMarkdown from "react-markdown";

interface MarkdownComponentProps {
  markdown: string;
}

export default function MarkdownComponent({
  markdown,
}: MarkdownComponentProps) {
  return (
    <section className="markdown">
      <ReactMarkdown>{markdown}</ReactMarkdown>
    </section>
  );
}
