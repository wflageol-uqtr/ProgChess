import { useEffect, useRef } from "react";

interface VerticalResizableProps {
  children: any;
  height: number;
  setHeight: any;
  setDisabledSelect: any;
}

export default function VerticalResizable({
  children,
  height,
  setHeight,
  setDisabledSelect,
}: VerticalResizableProps) {
  const isResized = useRef(false);

  useEffect(() => {
    const handleMouseMove = (e: MouseEvent) => {
      if (!isResized.current) return;

      setHeight((prev: number) => Math.max(prev + e.movementY, 100));
    };

    const handleMouseUp = () => {
      isResized.current = false;
      setDisabledSelect(false);
    };

    window.addEventListener("mousemove", handleMouseMove);
    window.addEventListener("mouseup", handleMouseUp);

    return () => {
      window.removeEventListener("mousemove", handleMouseMove);
      window.removeEventListener("mouseup", handleMouseUp);
    };
  }, []);

  return (
    <div className="flex flex-col" style={{ height: `${height}px` }}>
      <div className="flex-grow overflow-auto">{children}</div>
      <div
        className="h-2 cursor-row-resize"
        onMouseDown={() => {
          isResized.current = true;
          setDisabledSelect(true);
        }}
      />
    </div>
  );
}
