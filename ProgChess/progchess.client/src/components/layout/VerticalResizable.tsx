import { useEffect, useRef, useState } from "react";

export default function VerticalResizable({ children }: any) {
  const isResized = useRef(false);

  const [height, setHeight] = useState(225);

  useEffect(() => {
    window.addEventListener(
      "mousemove",
      (e) => {
        if (!isResized.current) {
          return;
        }

        setHeight((previousHeight) => previousHeight + e.movementY / 2);
      },
      []
    );

    window.addEventListener("mouseup", () => {
      isResized.current = false;
    });
  }, []);

  return (
    <div className="col-span-2 " style={{ height: `${height / 16}rem` }}>
      <div
        className="h-2 cursor-row-resize"
        onMouseDown={() => {
          isResized.current = true;
        }}
      ></div>{" "}
      {children}
    </div>
  );
}
