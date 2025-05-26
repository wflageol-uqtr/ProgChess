import { useEffect, useRef, useState } from "react";

interface BoxCardProps {
  children: React.ReactNode;
}

export default function HorizontalResizable({ children }: BoxCardProps) {
  const isResized = useRef(false);
  const [width, setWidth] = useState(window.innerWidth / 2);

  useEffect(() => {
    window.addEventListener("mousemove", (e) => {
      if (!isResized.current) {
        return;
      }
      setWidth((previousWidth) => previousWidth + e.movementX / 2);
    });

    window.addEventListener("mouseup", () => {
      isResized.current = false;
    });
  }, []);

  return (
    <div className="flex">
      <div style={{ width: `${width / 16}rem` }}>{children}</div>

      {/* Resizable element */}
      <div
        onMouseDown={() => {
          isResized.current = true;
        }}
        className="w-2 cursor-col-resize"
      ></div>
    </div>
  );
}
