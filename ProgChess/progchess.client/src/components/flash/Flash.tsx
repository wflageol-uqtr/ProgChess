import FlashError from "./FlashError";

const FlashType = {
  ERROR: "error",
  SUCCESS: "succes",
} as const;

type FlashTypeKey = (typeof FlashType)[keyof typeof FlashType];

interface FlashProps {
  message?: string;
  type: FlashTypeKey;
}

function Flash({ message, type }: FlashProps) {
  if (!message) return null;

  if (type == FlashType.ERROR) {
    return <FlashError message={message} />;
  }
  return <></>;
}

export default Flash;
