import { useEffect, useRef, useState } from "react";

export default function VerticalResizable({ children }: any) {
  const isResized = useRef(false);
  const [height, setHeight] = useState(300); // default height in px

  useEffect(() => {
    const handleMouseMove = (e: MouseEvent) => {
      if (!isResized.current) return;

      setHeight((prev) => Math.max(prev + e.movementY, 100)); // Set a min height of 100px
    };

    const handleMouseUp = () => {
      isResized.current = false;
    };

    window.addEventListener("mousemove", handleMouseMove);
    window.addEventListener("mouseup", handleMouseUp);

    return () => {
      window.removeEventListener("mousemove", handleMouseMove);
      window.removeEventListener("mouseup", handleMouseUp);
    };
  }, []);

  return (
    <div className="col-span-2 flex flex-col" style={{ height: `${height}px` }}>
      <div
        className="h-2 cursor-row-resize"
        onMouseDown={() => (isResized.current = true)}
      />
      <div className="flex-grow overflow-auto">{children}</div>
    </div>
  );
}
